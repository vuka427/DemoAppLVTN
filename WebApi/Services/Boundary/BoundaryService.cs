using Application.Interface.IDomainServices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Implementation.DomainServices
{
    public class Boundary
    {
        public int Id { get; set; }
        public string Name { get; set; }

    }

    public class Province : Boundary
    {
        public List<District> Districts { get; set; }
    }

    public class District : Boundary
    {
        public string Level { get; set; }
        public List<Wards> Wards { get; set; }
    }

    public class Wards : Boundary
    {
        public string Level { get; set; }
    }

    public class BoundaryService : IBoundaryService
    {

        public List<Province> TinhVN { get; set; }
        IWebHostEnvironment _env;

        public BoundaryService(IWebHostEnvironment env)
        {
            _env=env;

            string DataPath = Path.Combine(_env.ContentRootPath, "Uploads/DiaGioiHanhChinhVN.json");// đường dẫn 

            using (StreamReader sr = File.OpenText(DataPath))
            {
                var obj = sr.ReadToEnd();
                TinhVN = JsonConvert.DeserializeObject<List<Province>>(obj);
            }
        }

        public string GetAddress(int province, int district, int wards)
        {

            

            var Tinh = TinhVN.Where(t => t.Id == province).FirstOrDefault();
            if (Tinh == null) return string.Empty;
            var Huyen = Tinh.Districts.Where(h => h.Id == district).FirstOrDefault();
            if (Huyen == null) return string.Empty;
            var Xa = Huyen.Wards.Where(x => x.Id ==wards).FirstOrDefault();
            if (Xa == null) return string.Empty;

            StringBuilder Address = new StringBuilder();

            Address.Append(Xa.Name + ", ");
            Address.Append(Huyen.Name + ", ");
            Address.Append(Tinh.Name);

            return Address.ToString();
        }
    }
}
