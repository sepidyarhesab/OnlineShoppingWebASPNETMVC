using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrdersDatabase.Models;

namespace WebApplicationHamtOrders
{
    public class VersionControl
    {

        public class VersionViewModel
        {
            public bool IsOk { get; set; }
            public string ApplicationVersionDatabase { get; set; }
            public string DatabaseVersionDatabase { get; set; }
            public string ApplicationVersionAssembly { get; set; }
            public string DatabaseVersionAssembly { get; set; }
            public string Messages { get; set; }
        }
        public VersionViewModel ChechVersion()
        {
            var vm = new VersionViewModel();
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            System.Diagnostics.FileVersionInfo fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location);
            string database = fvi.FileVersion;
            string application = fvi.ProductVersion;
            vm.DatabaseVersionAssembly = database;
            vm.ApplicationVersionAssembly = application;
            var db = new Orders_Entities();
            var _apptype = Guid.Parse("aab6e40a-290b-4ef5-a0d4-d6a5bcff9791");
            var _datatype = Guid.Parse("23d34f8b-a0c2-4fc5-819c-1d9dd0205689");
            var _application = db.Table_Version.FirstOrDefault(c => c.Type == _apptype);
            var _databsae = db.Table_Version.FirstOrDefault(c => c.Type == _datatype);
            if (_application != null)
            {
                var versionApplication = _application.Major + "." + _application.Minor + "." + _application.Build + "." +
                                         _application.Revision;
                vm.ApplicationVersionDatabase = versionApplication;
                if (_databsae != null)
                {
                    var versionDatabase = _databsae.Major + "." + _databsae.Minor + "." + _databsae.Build + "." +
                                          _databsae.Revision;
                    vm.DatabaseVersionDatabase = versionDatabase;
                    if (application == versionApplication)
                    {
                        if (database == versionDatabase)
                        {
                            vm.IsOk = true;
                            vm.Messages = "Database Version : " + versionDatabase + "&" + "Application Version : " +
                                          versionApplication;
                        }
                        else
                        {
                            vm.IsOk = false;
                            vm.Messages = "Database Version : " + versionDatabase + "&" + "Application Version : " +
                                          versionApplication;
                        }
                    }
                    else
                    {
                        if (database == versionDatabase)
                        {
                            vm.IsOk = false;
                            vm.Messages = "Database Version : " + versionDatabase + "&" + "Application Version : " +
                                          versionApplication;
                        }
                        else
                        {
                            vm.IsOk = false;
                            vm.Messages = "Database Version : " + versionDatabase + "&" + "Application Version : " +
                                          versionApplication;
                        }
                    }

                }
                else
                {
                    vm.IsOk = false;
                    vm.Messages = "Database : " + "Not Found" + "&" + "Application  : " +
                                  "Found";
                }
            }
            else
            {
                vm.IsOk = false;
                vm.Messages = "Database : " + "Not Found" + "&" + "Application  : " +
                              "Not Found";
            }

            return vm;
        }
    }
}
