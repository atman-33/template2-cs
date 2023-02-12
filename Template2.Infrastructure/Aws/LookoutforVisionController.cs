using Amazon;
using Amazon.LookoutforVision;
using Amazon.LookoutforVision.Model;
using Amazon.Runtime;
using Amazon.Runtime.CredentialManagement;
using System.Diagnostics;

namespace Template2.Infrastructure.Aws
{
    /// <summary>
    /// AWS Lookout for Visionを扱うクラス
    /// 利用するにあたり下記を実施しておく必要有り
    /// 1. AWS CLIインストール（AWS Config生成）
    /// </summary>
    public class LookoutforVisionController
    {
        const string ProfileName = "l4v_profile";

        private AmazonLookoutforVisionClient? _l4vClient;
        private string _projectName = String.Empty;
        private string _modelVersion = String.Empty;
        private int _minInferenceUnits;
        private string _clientToken = "l4v-client-token";

        public LookoutforVisionController(string projectName, string modelVersion, int minInferenceUnits)
        {
            AmazonLookoutforVisionConfig config = new AmazonLookoutforVisionConfig();
            config.RegionEndpoint = RegionEndpoint.APNortheast1;

            //// プロキシの設定を行う場合は、下記を設定
            //config.ProxyHost = "XXX.X.X.X";
            //config.ProxyPort = XXXX;

            SharedCredentialsFile credentialsFile = new SharedCredentialsFile();
            CredentialProfile? profile = null;

            if (credentialsFile.TryGetProfile(ProfileName, out profile) == false)
            {
                Console.WriteLine("プロファイルの取得に失敗しました。");
                return;
            }

            AWSCredentials? awsCredentials = null;
            if (AWSCredentialsFactory.TryGetAWSCredentials(profile, credentialsFile, out awsCredentials) == false)
            {
                Console.WriteLine("認証情報の生成に失敗しました。");
            }

            _l4vClient = new AmazonLookoutforVisionClient(awsCredentials, config);

            _projectName = projectName;
            _modelVersion = modelVersion;
            _minInferenceUnits = minInferenceUnits;
        }

        public async void StartModelAsync()
        {
            if (_l4vClient == null)
            {
                return;
            }

            StartModelRequest request = new StartModelRequest();
            request.ProjectName = _projectName;
            request.ModelVersion = _modelVersion;
            request.MinInferenceUnits = _minInferenceUnits;
            request.ClientToken = _clientToken;

            await Task.Run(() => _l4vClient.StartModelAsync(request));
        }

        public async Task<String> DescribeModelStatusAsync()
        {
            if (_l4vClient == null)
            {
                return String.Empty;
            }

            DescribeModelRequest request = new DescribeModelRequest();
            request.ProjectName = _projectName;
            request.ModelVersion = _modelVersion;

            var result = await Task.Run(() => _l4vClient.DescribeModelAsync(request));

            Debug.WriteLine("AWS L4v Status: " + result.ModelDescription.Status);
            Debug.WriteLine("AWS L4v StatusMessage: " + result.ModelDescription.StatusMessage);
            return result.ModelDescription.Status;
        }

        public async Task<bool?> DetectAnomaliesIsAnomalousAsync(string path)
        {
            if (_l4vClient == null)
            {
                return null;
            }

            DetectAnomaliesRequest request = new DetectAnomaliesRequest();
            request.ProjectName = _projectName;
            request.ModelVersion = _modelVersion;
            request.Body = new FileStream(path, FileMode.Open);
            request.ContentType = "image/jpeg";

            var result = await Task.Run(() => _l4vClient.DetectAnomaliesAsync(request));

            request.Body.Dispose();
            return result.DetectAnomalyResult.IsAnomalous;
        }
        public async Task<float?> DetectAnomaliesConfidenceAsync(string path)
        {
            if (_l4vClient == null)
            {
                return null;
            }

            DetectAnomaliesRequest request = new DetectAnomaliesRequest();
            request.ProjectName = _projectName;
            request.ModelVersion = _modelVersion;
            request.Body = new FileStream(path, FileMode.Open);
            request.ContentType = "image/jpeg";

            var result = await Task.Run(() => _l4vClient.DetectAnomaliesAsync(request));

            request.Body.Dispose();
            return result.DetectAnomalyResult.Confidence;
        }

        public async void StopModelAsync()
        {
            if (_l4vClient == null)
            {
                return;
            }

            StopModelRequest request = new StopModelRequest();
            request.ProjectName = _projectName;
            request.ModelVersion = _modelVersion;
            request.ClientToken = _clientToken;

            await Task.Run(() => _l4vClient.StopModelAsync(request));
        }
    }
}
