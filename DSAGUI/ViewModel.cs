using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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

        private UserKeyPair _keyPair;
        public UserKeyPair KeyPair
        {
            get => _keyPair;
            set
            {
                SetProperty(ref _keyPair, value);
            }
        }

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


        public RelayCommand GenerateDomainParameters { get; set; }

        public RelayCommand GenerateKeyPair { get; set; }

        private IDataProvider _dataProvider;


        public ViewModel()
        {
            //DomainParameters = _dsaParametersGenerator.GenerateParameters(1024, 160, 160);
            GenerateDomainParameters = new RelayCommand(() =>
            {
                DomainParameters = _dsaParametersGenerator.GenerateParameters(1024, 160, 160);
                GenerateKeyPair.RaiseCanExecuteChanged();
                _keyGenerator = new UserKeyGenerator(DomainParameters);
            });


            GenerateKeyPair = new RelayCommand(() =>
            {
                _keyGenerator.SystemParameters = DomainParameters;
                KeyPair = _keyGenerator.GenerateKeyPair();
                SignFileCommand.RaiseCanExecuteChanged();
                Algorithm = new DsaAlgorithm(DomainParameters);
            }, () => DomainParameters != null );

            // TO DO iniialize instantion of DSA ALgorithm
            //DomainParameters.HashFunction = new Hasher(Hasher.HashImplementation.Md5);
            //_algorythym = new DsaAlgorithm(DomainParameters);

            SelectFileToSignCommand = new RelayCommand(SelectFileToSign);
            SelectFileToVerifyCommand = new RelayCommand(SelectFileToVerify);
            SignFileCommand = new RelayCommand(SigningFile, () => _keyPair != null && SigningData != null );
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

            SignFileCommand.RaiseCanExecuteChanged();
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
                try
                {
                    _dataProvider = new FileDataProvider(_fileToSignPath);
                    SigningData = _dataProvider.GetData();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message, "Error!");
                }
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
                try
                {
                    _dataProvider = new FileDataProvider(_fileToVerifyPath);
                    VerifyingData = _dataProvider.GetData();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message, "Error!");
                }
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
        public RelayCommand SignFileCommand { get; set; }

        private void SigningFile()
        {
            try
            {
                _signatureToSign = Algorithm.Sign(SigningData, KeyPair.PrivateKey);
                _signatureToSign.ToBinaryStringFile();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error!");
            }
            //var bytesToSign = _dataProvider.GetData();
        }

        public RelayCommand VerifyFileCommand { get; set; }

        private void VerifyFile()
        {
            if (KeyPair == null) KeyPair = new UserKeyPair(0, 0);

            _signatureToVerify = Signature.FromBinaryString(VerifyingSignFilePath);
            IsTheSameFile = Algorithm.Verify(VerifyingData, _signatureToVerify, KeyPair.PublicKey);
            MessageBox.Show($"The document is signed {(IsTheSameFile ? "" : "in")}correctly", IsTheSameFile ? "CORRECT": "FALSE");
        }
    }
}
