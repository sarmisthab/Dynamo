﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Dynamo.Utilities;
using Greg.Responses;
using Microsoft.Practices.Prism.ViewModel;

namespace Dynamo.PackageManager
{
    public class DynamoPackageDownload : NotificationObject
    {
        public enum State
        {
            Uninitialized, Downloading, Downloaded, Installing, Installed, Error
        }

        private string _errorString = "";
        public string ErrorString { get { return _errorString; } set { _errorString = value; RaisePropertyChanged("ErrorString"); } }

        private State _downloadState = State.Uninitialized;

        public State DownloadState
        {
            get { return _downloadState; }
            set
            {
                _downloadState = value;
                RaisePropertyChanged("DownloadState");
            }
        }

        public PackageHeader Header { get; private set; }
        public string Name { get { return Header.name; } }

        private string _downloadPath;
        public string DownloadPath { get { return _downloadPath; } set { _downloadPath = value; RaisePropertyChanged("DownloadPath"); } }

        private string _versionName;
        public string VersionName { get { return _versionName; } set { _versionName = value; RaisePropertyChanged("VersionName"); } }

        private DynamoPackageDownload()
        {
            
        }

        public DynamoPackageDownload(PackageHeader header, string version)
        {
            this.Header = header;
            this.DownloadPath = "";
        }

        public void Start()
        {
            dynSettings.Controller.PackageManagerClient.Download(this);
        }

        public void Error(string errorString)
        {
            this.DownloadState = State.Error;
            this.ErrorString = errorString;
        }

        public void Done( string filePath )
        {
            this.DownloadState = State.Downloaded;
            this.DownloadPath = filePath;
        }

        public string MakeInstallDirectory()
        {
            // assembly path, packages
        }

        public bool Extract( out DynamoInstalledPackage pkg )
        {
            if (this.DownloadState != State.Downloaded)
            {
                pkg = null;
                return false;
            }

            // unzip, place files
            var unzippedDirectory = Greg.Utility.FileUtilities.UnZip(DownloadPath);

            // provide handle to installed package 
            pkg = new DynamoInstalledPackage( unzippedDirectory, Header, VersionName );

            return true;
        }

        // cancel, install, redownload

    }

    public class DynamoInstalledPackage
    {
        public DynamoInstalledPackage(string directory, PackageHeader header, string versionName )
        {
            
        }

        // do everything necessary to make the host aware of the package
        // may require a restart
        public bool RegisterWithHost()
        {
            return false;
        }

        public DynamoInstalledPackage FromXML()
        {
            // open and deserialize a package
        }

        // header
        // location of all files
        public void Uninstall()
        {
            // remove this package completely
        }

        public DynamoInstalledPackage ToXML()
        {
            // open and deserialize a package
            // 
        }
    }



}