using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartdustApp.Business.Core.Model
{
    public class DocumentModel
    {
        public int ID { get; set; }
        // Image Name 
        public string Name { get; set; }
        //Image Type
        public string FileType { get; set; }
        //Image Byte
        public byte[] DataFiles { get; set; }
    }
}
