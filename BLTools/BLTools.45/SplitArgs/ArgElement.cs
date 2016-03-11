using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLTools {
  /// <summary>
  /// Single element of arguments : Id(position), Name, Value
  /// </summary>
  public class ArgElement : IEquatable<ArgElement> {
    public int Id {
      get;
      set;
    }
    public string Name {
      get;
      set;
    }
    public string Value {
      get;
      set;
    }

    public ArgElement(int id, string name, string value) {
      Id = id;
      Name = name;
      Value = value;
    }

    public bool Equals(ArgElement other) {
      if (other == null) {
        return false;
      }
      return (Name == other.Name && Value == other.Value);
    }

    public override bool Equals(object obj) {
      ArgElement other = obj as ArgElement;

      if (obj == null) {
        return false;
      }

      return (Name == other.Name && Value == other.Value);
    }

    public override int GetHashCode() {
      return Id.GetHashCode() | Name.GetHashCode() | Value.GetHashCode();
    }
  }
}
