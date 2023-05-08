using System.Data.SQLite;
using Template2.Domain.Entities;
using Template2.Domain.Repositories;

namespace Template2.Infrastructure.SQLite
{
    internal class PageMstSQLite : IPageMstRepository
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

            return SQLiteHelper.Query(sql,
                reader =>
                {
                    return new PageMstEntity(
                        Convert.ToInt32(reader["page_id"]),
						Convert.ToString(reader["page_name"]) ?? string.Empty,
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
 (@page_id,
  @page_name,
  @movie_link,
  @image_folder_link,
  @image_page_no,
  @slide_waiting_time,
  @note1,
  @note2,
  @note3)
";
            string update = @"
UPDATE tmp_page_mst
SET 
  page_name = @page_name,
  movie_link = @movie_link,
  image_folder_link = @image_folder_link,
  image_page_no = @image_page_no,
  slide_waiting_time = @slide_waiting_time,
  note1 = @note1,
  note2 = @note2,
  note3 = @note3
WHERE
  page_id = @page_id
";
            var args = new List<SQLiteParameter>
            {
                new SQLiteParameter("@page_id", entity.PageId.Value),
				new SQLiteParameter("@page_name", entity.PageName.Value),
				new SQLiteParameter("@movie_link", entity.MovieLink.Value),
				new SQLiteParameter("@image_folder_link", entity.ImageFolderLink.Value),
				new SQLiteParameter("@image_page_no", entity.ImagePageNo.Value),
				new SQLiteParameter("@slide_waiting_time", entity.SlideWaitingTime.Value),
				new SQLiteParameter("@note1", entity.Note1.Value),
				new SQLiteParameter("@note2", entity.Note2.Value),
				new SQLiteParameter("@note3", entity.Note3.Value)
            };

            SQLiteHelper.Execute(insert, update, args.ToArray());
        }

        public void Delete(PageMstEntity entity)
        {
            string delete = @"
DELETE FROM tmp_page_mst WHERE page_id = @page_id
";

            var args = new List<SQLiteParameter>
            {
                new SQLiteParameter("@page_id", entity.PageId.Value)
            };

            SQLiteHelper.Execute(delete, args.ToArray());
        }

        public int GetNextId()
        {
            string sql = @"
SELECT MAX(page_id) AS max_page_id FROM tmp_page_mst
";

            var maxId =  SQLiteHelper.QuerySingle<int>(
                sql,
                reader =>
                {
                    return reader["max_page_id"] != DBNull.Value ? Convert.ToInt32(reader["max_page_id"]) : 0;
                }, 
                0);

            return maxId + 1;
        }
    }
}
