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
        [PrimaryKey, AutoIncrement]

        public int ID { get; set; }

        [Unique]

        public String appointmentName { get; set; }

        [NotNull]
        public DateTime appointmentDate { get; set; }

        [NotNull]
        public DateTime StartTime { get; set; }

        [NotNull]
        public DateTime EndTime { get; set; }
    }
}
