using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using DSAAlgorythm.Model;
using DSAAlgorythm.Services;
using Microsoft.Win32;
using DSAGUI;
using WPFGUI;
using DSAAlgorythm.Data;
using DSAAlgorythm;


namespace DSAGUI
{
    public class ViewModel : BindableBase
    {
        private DsaParametersGenerator _dsaParametersGenerator = new DsaParametersGenerator();
        private UserKeyGenerator _keyGenerator;

        public string SignatureFilePath { get; set; }

        public UserKeyPair KeyPair { get; set; }

        public DsaSystemParameters DomainParameters { get; set; }

        public ICommand GenerateDomainParameters { get; set; }

        public ICommand GenerateKeyPair { get; set; }

        public ICommand SignFile { get; set; }

        private IDataProvider _dataProvider;
        public DsaAlgorithm Algorithm { get; set; }

        public ViewModel()
        { //TODO initialize DomainParameters
            //DomainParameters = _dsaParametersGenerator.GenerateParameters(1024, 160, 160);
            GenerateDomainParameters = new RelayCommand(() =>
            {
                DomainParameters = _dsaParametersGenerator.GenerateParameters(1024, 160, 160);
                (GenerateKeyPair as RelayCommand).RaiseCanExecuteChanged();
                _keyGenerator = new UserKeyGenerator(DomainParameters);
            });


            GenerateKeyPair = new RelayCommand(() =>
            {
                _keyGenerator.SystemParameters = DomainParameters;
                KeyPair = _keyGenerator.GenerateKeyPair();
            }, () => DomainParameters != null );

            // TO DO iniialize instantion of DSA ALgorithm
            //DomainParameters.HashFunction = new Hasher(Hasher.HashImplementation.Md5);
            //_algorythym = new DsaAlgorithm(DomainParameters);

            SelectFileToSignCommand = new RelayCommand(SelectFileToSign);
            SelectFileToVerifyCommand = new RelayCommand(SelectFileToVerify);
            SignFile = new RelayCommand(SigningFile);
            //SignFile = new RelayCommand(() => { File.Open(FileToSignPath, FileMode.Open); }); //todo


        }

        #region DataSource
        public RelayCommand SelectFileToSignCommand { get; set; }
        private void SelectFileToSign()
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Title = "Select file to sign";

            if (fileDialog.ShowDialog() == true)
            {
                FileToSignPath = fileDialog.FileName;
            }
        }

        private string _fileToSignPath;

        public string FileToSignPath
        {
            get => _fileToSignPath;
            set
            {
                SetProperty(ref _fileToSignPath, value);
                _dataProvider = new FileDataProvider(_fileToSignPath);
            }
        }

        public RelayCommand SelectFileToVerifyCommand { get; set; }

        private void SelectFileToVerify()
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Title = "Select file to verify with given sign";
            if(fileDialog.ShowDialog() == true)
            {
                FileToVerifyPath = fileDialog.FileName;
            }
        }
        private string _fileToVerifyPath;

        public string FileToVerifyPath
        {
            get => _fileToVerifyPath;
            set
            {
                SetProperty(ref _fileToSignPath, value);
                // TODO add blocking bad user's behaviour SelectFileToSignCommand.RaiseCanExecuteChanged();
            }
        }

        public RelayCommand SigningFileCommand { get; set; }
        #endregion
        private void SigningFile()
        {
            Algorithm = new DsaAlgorithm(DomainParameters);
            var bytesToSign = _dataProvider.GetData();
            Signature signature = Algorithm.Sign(bytesToSign, KeyPair.PrivateKey);
            signature.ToBinaryStringFile();
        }
    }
}
