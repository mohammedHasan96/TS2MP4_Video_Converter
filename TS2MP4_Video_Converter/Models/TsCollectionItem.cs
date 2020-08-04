using DevExpress.Mvvm.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TS2MP4_Video_Converter.Models
{
    public enum ConvertStatus { Waiting, InProgress, Success, Failed }
    public class TsFilesCollectionItem
    {

        public static TsFilesCollectionItem Create(Action<TsFilesCollectionItem> onDelete)
            => ViewModelSource.Create(() => new TsFilesCollectionItem(onDelete));
        public virtual string FileName { get; set; }
        public virtual string Size { get; set; }
        [DevExpress.Mvvm.DataAnnotations.BindableProperty(OnPropertyChangedMethodName = nameof(StatusChanged))]
        public virtual ConvertStatus Status { get; set; }
        public virtual bool DeleteEnabled { get; set; } = true;
        public virtual bool Processed { get; set; } = false;

        private Action<TsFilesCollectionItem> onDelete;
        public TsFilesCollectionItem(Action<TsFilesCollectionItem> onDelete = null)
        {
            this.onDelete = onDelete;
        }

        protected void StatusChanged(ConvertStatus oldValue)
        {
            if (Status == ConvertStatus.InProgress)
                DeleteEnabled = false;
            else
                DeleteEnabled = true;
        }

        public void Delete()
        {
            onDelete?.Invoke(this);
        }
    }
}
