using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EDF
{
  public  class BCEVW
    {

      public static List<charge> reMoveUnFeasible(List <charge> theBook)
      {
          List<charge> toDelete = new List<charge>();
          for (int i = 0; i < theBook.Count; i++)
          {
              if (theBook[i].getCheckValue() > 1)
                  toDelete.Add(theBook [i]);
          }
          for (int i = 0; i < toDelete.Count; i++)
          {
              theBook.Remove(toDelete [i]);
          }
          return theBook;
      }
    }
}
