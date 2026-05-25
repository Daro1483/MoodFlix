using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class BackupService
    {
        private DAL.BackupRepository backup = new DAL.BackupRepository();

        public int CreateBackup(string path)
        {
            return backup.CreateBackup(path);
        }

        public void RestoresBackup(string path)
        {
            backup.RestoresBackup(path);
        }
    }
}

