using System.Collections.Generic;
using Data;

namespace Interfaces.ReadOnly
{
    public interface IReadOnlyDialogs
    {
        public DialogStrokeData GetDataById(int id);

        public List<DialogStrokeData> GetDataByIds(List<int> ids);
    }
}