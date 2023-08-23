using dto = Domain.DTO_s;
using orm = Infrastructure.Data;

namespace Domain.MAPPER
{
    public class MapImage
    {
        public static dto.RoomImage MAP(orm.RoomImages obj)
        {
            var image = new dto.RoomImage();
            if (obj != null)
            {
                image = new dto.RoomImage()
                {
                    ImageUrl = obj.ImageUrl
                };
            }
            return image;
        }

        public static List<dto.RoomImage> MAP(List<orm.RoomImages> obj)
        {
            var list = new List<dto.RoomImage>();
            if(obj != null)
            {
                foreach(var item in obj)
                {
                    list.Add(MAP(item));
                }
            }
            return list;
        }

        public static orm.RoomImages MAP(dto.RoomImage obj)
        {
            var image = new orm.RoomImages();
            if (obj != null)
            {
                image = new orm.RoomImages()
                {
                    ImageUrl = obj.ImageUrl,
                    RoomId = obj.RoomId
                };
            }
            return image;
        }

        public static List<orm.RoomImages> MAP(List<dto.RoomImage> obj)
        {
            var list = new List<orm.RoomImages>();
            if (obj != null)
            {
                foreach (var item in obj)
                {
                    list.Add(MAP(item));
                }
            }
            return list;
        }
    }
}
