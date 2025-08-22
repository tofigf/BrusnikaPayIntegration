using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrusnikaPayIntegration.Models
{
    public class ApiResponse<T>
    {
        public ApiResult Result { get; set; } = new();
        public T? Data { get; set; }
        public int TotalNumberRecords { get; set; }
    }
}
