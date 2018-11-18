using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using DSAAlgorythm.Model;
using DSAAlgorythm.Services;
using WPFGUI;

namespace DSAGUI
{
    public class ViewModel
    {
        private DsaParametersGenerator _dsaParametersGenerator = new DsaParametersGenerator();
        private UserKeyGenerator _keyGenerator;

        public string FileToSignPath { get; set; }

        public string SignatureFilePath { get; set; }

        public UserKeyPair KeyPair { get; set; }

        public DsaSystemParameters DomainParameters { get; set; }

        public ICommand GenerateDomainParameters { get; set; }

        public ICommand GenerateKeyPair { get; set; }

        public ICommand SignFile { get; set; }

        public ViewModel()
        {
            _keyGenerator = new UserKeyGenerator(DomainParameters);

            GenerateDomainParameters = new RelayCommand(() =>
            {
                _dsaParametersGenerator.GenerateParameters(1024, 160, 160);
                (GenerateKeyPair as RelayCommand).RaiseCanExecuteChanged();
            });

            GenerateKeyPair = new RelayCommand(() =>
            {
                _keyGenerator.SystemParameters = DomainParameters;
                _keyGenerator.GenerateKeyPair();
            }, () => DomainParameters != null );

            SignFile = new RelayCommand(() => { File.Open(FileToSignPath, FileMode.Open); }); //todo
        }
    }
}
