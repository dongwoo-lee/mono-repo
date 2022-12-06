using M30.AudioFile.Common;
using M30.AudioFile.DAL.Dao;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAMBrowser.Foundation
{
    public static class ConnectNetDrive 
    {
        private static Dictionary<string, csNetDrive> netDrivelist = new Dictionary<string, csNetDrive>();

        public static NetDriveCode Connect(string directory, string userId, string pass)
        {
            csNetDrive netDrive = new csNetDrive();

            var result = netDrive.SetRemoteConnection(directory, userId, pass);
            
            if (result == NetDriveCode.NO_ERROR)
            {
                var host = CommonUtility.GetHost(directory);
                if (!netDrivelist.ContainsKey(host))
                    netDrivelist.Add(host, netDrive);
            }

            return result;
        }

        public static void AllDisConnect()
        {
            foreach(var netDrive in netDrivelist)
            {
                try
                {
                    var result = netDrive.Value.CencelRemoteServer(netDrive.Key);
                }
                catch(Exception ex)
                {
                    
                }
            }
        }
    }
}
