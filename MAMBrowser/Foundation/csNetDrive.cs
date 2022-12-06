using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MAMBrowser.Foundation
{
    internal class csNetDrive : IDisposable
    {
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct NETRESOURCE
        {
            public uint dwScope;
            public uint dwType;
            public uint dwDisplayType;
            public uint dwUsage;
            public string lpLocalName;
            public string lpRemoteName;
            public string lpComment;
            public string lpProvider;
        }

        // Net Drive 연결
        [DllImport("mpr.dll", CharSet = CharSet.Auto)]
        public static extern int WNetUseConnection(
            IntPtr hwndOwner,
            [MarshalAs(UnmanagedType.Struct)] ref NETRESOURCE lpNetResource,
            string lpPassword,
            string lpUserID,
            uint dwFlags,
            StringBuilder lpAccessName,
            ref int lpBufferSize,
            out uint lpResult);

        // Net Drive 연결
        public NetDriveCode SetRemoteConnection(string strRemoteConnectString, string strRemoteUserID, string strRemotePWD, string strLocalName = null)
        {
            int capacity = 64;
            uint resultFlags = 0;
            uint flags = 0;

            try
            {
                if ((strRemoteConnectString != "" || strRemoteConnectString != string.Empty) &&
                   (strRemoteUserID != "" || strRemoteUserID != string.Empty) &&
                   (strRemotePWD != "" || strRemotePWD != string.Empty))
                {
                    System.Text.StringBuilder sb = new System.Text.StringBuilder(capacity);
                    NETRESOURCE ns = new NETRESOURCE();
                    ns.dwType = 1;
                    ns.lpLocalName = strLocalName;                 // 로컬 드라이브명(null 이면 자동)
                    ns.lpRemoteName = @strRemoteConnectString;
                    ns.lpProvider = null;

                    int result = WNetUseConnection(IntPtr.Zero, ref ns, strRemotePWD, strRemoteUserID, flags,
                                        sb, ref capacity, out resultFlags);

                    return (NetDriveCode)result;
                }
                else
                {
                    return NetDriveCode.Fail;
                }
            }
            catch
            {
                throw;
                //return -1;
            }
        }

        // Net Drive 해제
        [DllImport("mpr.dll", EntryPoint = "WNetCancelConnection2", CharSet = CharSet.Auto)]
        public static extern int WNetCancelConnection2(string lpName, int dwFlags, int fForce);

        // Net Drive 해제
        public NetDriveCode CencelRemoteServer(string strRemoteConnectString)
        {
            int result;

            try
            {
                result = WNetCancelConnection2(strRemoteConnectString, 1, 1);
            }
            catch
            {
                result = 0;
                throw;
            }

            return (NetDriveCode)result;
        }

        public void Dispose()
        {

        }
    }

    public enum NetDriveCode
    {
        Fail = -1,
        NO_ERROR = 0,
        ERROR_ACCESS_DENIED = 5,
        ERROR_NO_NET_OR_BAD_SERVER = 53,
        ERROR_NETWORK_BUSY = 54,
        ERROR_UNEXP_NET_ERR = 59,
        ERROR_BAD_DEV_TYPE = 66,
        ERROR_BAD_NET_NAME = 67,
        ERROR_ALREADY_ASSIGNED = 85,
        ERROR_INVALID_PASSWORD = 86,
        ERROR_INVALID_PARAMETER = 87,
        ERROR_BUSY = 170,
        ERROR_INVALID_ADDRESS = 487,
        ERROR_BAD_DEVICE = 1200,
        ERROR_DEVICE_ALREADY_REMEMBERED = 1202,
        ERROR_NO_NET_OR_BAD_PATH = 1203,
        ERROR_BAD_PROVIDER = 1204,
        ERROR_CANNOT_OPEN_PROFILE = 1205,
        ERROR_BAD_PROFILE = 1206,
        ERROR_EXTENDED_ERROR = 1208,
        ERROR_MULTIPLE_CONNECTION = 1219,
        ERROR_CANCELLED = 1223,
        ERROR_BAD_USER_OR_PASSWORD = 1326
    };
}
