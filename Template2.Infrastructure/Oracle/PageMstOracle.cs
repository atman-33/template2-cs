using Oracle.ManagedDataAccess.Client;
using Template2.Domain.Entities;
using Template2.Domain.Repositories;

namespace Template2.Infrastructure.Oracle
{
    internal class PageMstOracle : IPageMstRepository
    {
        public IReadOnlyList<PageMstEntity> GetData()
        {
            string sql = @"
SELECT
  page_id,
  page_name,
  movie_link,
  image_folder_link,
  image_page_no,
  slide_waiting_time,
  note1,
  note2,
  note3
FROM
  tmp_page_mst
";

            return OracleOdpHelper.Query(sql,
                reader =>
                {
                    return new PageMstEntity(
                        Convert.ToInt32(reader["page_id"]),
						Convert.ToString(reader["page_name"]),
						reader["movie_link"] != DBNull.Value ? Convert.ToString(reader["movie_link"]) : null,
						reader["image_folder_link"] != DBNull.Value ? Convert.ToString(reader["image_folder_link"]) : null,
						reader["image_page_no"] != DBNull.Value ? Convert.ToInt32(reader["image_page_no"]) : null,
						Convert.ToSingle(reader["slide_waiting_time"]),
						reader["note1"] != DBNull.Value ? Convert.ToString(reader["note1"]) : null,
						reader["note2"] != DBNull.Value ? Convert.ToString(reader["note2"]) : null,
						reader["note3"] != DBNull.Value ? Convert.ToString(reader["note3"]) : null
                        );
                });
        }

        public void Save(PageMstEntity entity)
        {
            string insert = @"
INSERT INTO tmp_page_mst
 (page_id,
  page_name,
  movie_link,
  image_folder_link,
  image_page_no,
  slide_waiting_time,
  note1,
  note2,
  note3)
VALUES
 (:page_id,
  :page_name,
  :movie_link,
  :image_folder_link,
  :image_page_no,
  :slide_waiting_time,
  :note1,
  :note2,
  :note3)
";
            string update = @"
UPDATE tmp_page_mst
SET 
  page_name = :page_name,
  movie_link = :movie_link,
  image_folder_link = :image_folder_link,
  image_page_no = :image_page_no,
  slide_waiting_time = :slide_waiting_time,
  note1 = :note1,
  note2 = :note2,
  note3 = :note3
WHERE
  page_id = :page_id
";
            var args = new List<OracleParameter>
            {
                new OracleParameter(":page_id", entity.PageId.Value),
				new OracleParameter(":page_name", entity.PageName.Value),
				new OracleParameter(":movie_link", entity.MovieLink.Value),
				new OracleParameter(":image_folder_link", entity.ImageFolderLink.Value),
				new OracleParameter(":image_page_no", entity.ImagePageNo.Value),
				new OracleParameter(":slide_waiting_time", entity.SlideWaitingTime.Value),
				new OracleParameter(":note1", entity.Note1.Value),
				new OracleParameter(":note2", entity.Note2.Value),
				new OracleParameter(":note3", entity.Note3.Value)
            };

            OracleOdpHelper.Execute(insert, update, args.ToArray());
        }



        public void Delete(PageMstEntity entity)
        {
            string delete = @"
DELETE FROM tmp_page_mst WHERE page_id = :page_id
";

            var args = new List<OracleParameter>
            {
                new OracleParameter(":page_id", entity.PageId.Value)
            };

            OracleOdpHelper.Execute(delete, args.ToArray());
        }

        public int GetNextId()
        {
            throw new NotImplementedException();
        }
    }
}
