using Altaaref.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Altaaref.Models
{
    public class HelpRequestComment : BaseViewModel
    {
        public int Id { get; set; }

        private string _comment;
        public string Comment
        {
            get { return _comment; }
            set { SetValue(ref _comment, value); }
        }

        private int _helpRequestId;
        public int HelpRequestId
        {
            get { return _helpRequestId; }
            set { SetValue(ref _helpRequestId, value); }
        }
    }
}
