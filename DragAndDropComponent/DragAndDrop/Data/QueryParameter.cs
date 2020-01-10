using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DragAndDrop.Data {
  public class QueryParameter {
    public QueryParameter ShallowCopy() {
      return (QueryParameter)MemberwiseClone();
    }

    public QueryParameter DeepCopy() {
      var cloneOfParam = ShallowCopy();
      cloneOfParam.Values = new List<object>();
      cloneOfParam.Values.AddRange(Values);
      return cloneOfParam;
    }

    public string Field { get; set; }
    public string Operator { get; set; }
    public List<object> Values { get; set; }

    public override string ToString() => $"{Field} {Operator} {string.Join(", ", Values)}";
  }
}
