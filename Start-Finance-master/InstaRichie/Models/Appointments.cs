using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite.Net.Attributes;


namespace StartFinance.Models
{
    class Appointments
    {
        [PrimaryKey]

        public string ID { get; set; }

        [Unique]

        public string appointmentName { get; set; }

        [NotNull]
        public string Date { get; set; }

        [NotNull]
        public string StartTime { get; set; }

        [NotNull]
        public string EndTime { get; set; }
    }
}
