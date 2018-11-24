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
using System.Windows;

namespace DSAGUI
{
    public class ViewModel : BindableBase
    {
        private DsaParametersGenerator _dsaParametersGenerator = new DsaParametersGenerator();
        private UserKeyGenerator _keyGenerator;

        public string SignatureFilePath { get; set; }

        public UserKeyPair KeyPair { get; set; }

        public DsaSystemParameters DomainParameters { get; set; }
        public DsaAlgorithm Algorithm { get; set; }

        private Signature _signatureToSign;

        private Signature _signatureToVerify;
        private bool _isTheSameFile;

        public bool IsTheSameFile
        {
            get { return _isTheSameFile; }
            set { _isTheSameFile = value; }
        }


        public ICommand GenerateDomainParameters { get; set; }

        public ICommand GenerateKeyPair { get; set; }

        private IDataProvider _dataProvider;


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
                Algorithm = new DsaAlgorithm(DomainParameters);
            }, () => DomainParameters != null );

            // TO DO iniialize instantion of DSA ALgorithm
            //DomainParameters.HashFunction = new Hasher(Hasher.HashImplementation.Md5);
            //_algorythym = new DsaAlgorithm(DomainParameters);

            SelectFileToSignCommand = new RelayCommand(SelectFileToSign);
            SelectFileToVerifyCommand = new RelayCommand(SelectFileToVerify);
            SignFileCommand = new RelayCommand(SigningFile);
            SignToVerifyCommand = new RelayCommand(SelectSignToVerifyFile);
            VerifyFileCommand = new RelayCommand(VerifyFile);


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

        public byte[] SigningData { get; set; }
        public byte[] VerifyingData { get; set; }

        private string _fileToSignPath;

        public string FileToSignPath
        {
            get => _fileToSignPath;
            set
            {
                SetProperty(ref _fileToSignPath, value);
                _dataProvider = new FileDataProvider(_fileToSignPath);
                SigningData = _dataProvider.GetData();
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
                SetProperty(ref _fileToVerifyPath, value);
                _dataProvider = new FileDataProvider(_fileToVerifyPath);
                VerifyingData = _dataProvider.GetData();
                // TODO add blocking bad user's behaviour SelectFileToSignCommand.RaiseCanExecuteChanged();
            }
        }
        private string _signToVerifyPath;
        public string VerifyingSignFilePath
        {
            get => _signToVerifyPath;
            set
            {
                SetProperty(ref _signToVerifyPath, value);
                //_dataProvider = new FileDataProvider(_signToVerifyPath);
            }
        }

        public RelayCommand SignToVerifyCommand { get; set; }

        private void SelectSignToVerifyFile()
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Title = "Select sign to verify file";
            if (fileDialog.ShowDialog() == true)
            {
                VerifyingSignFilePath = fileDialog.FileName;
            }
        }
        #endregion
        public ICommand SignFileCommand { get; set; }

        private void SigningFile()
        {

            //var bytesToSign = _dataProvider.GetData();
            _signatureToSign = Algorithm.Sign(SigningData, KeyPair.PrivateKey);
            _signatureToSign.ToBinaryStringFile();
        }

        public RelayCommand VerifyFileCommand { get; set; }

        private void VerifyFile()
        {
            try
            {
                _signatureToVerify = _signatureToSign.FromBinaryString(VerifyingSignFilePath);
            }
            catch (Exception ex)
            {

                MessageBox.Show("Podano głupi plik");
            }
            if( IsTheSameFile = Algorithm.Verify(VerifyingData, _signatureToVerify, KeyPair.PublicKey))
            {
                MessageBox.Show("File Succesfully verified");
            }
            else
            {
                MessageBox.Show("File is not original, Caution!");
            }
            Console.WriteLine("");
        }
    }
}
