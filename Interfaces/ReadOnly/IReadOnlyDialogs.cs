using System.Collections.Generic;
using Data;

namespace Interfaces.ReadOnly
{
    public interface IReadOnlyDialogs
    {
        public DialogStrokeData GetDataById(int id);
        public string GetChoiceDataById(int id);

        public List<DialogStrokeData> GetDataByIds(List<int> ids);

        public List<string> GetChoiceDataByIds(List<int> ids);
        
    }
}