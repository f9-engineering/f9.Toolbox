using System;
using System.Collections.Generic;
using System.Linq;

namespace f9.Toolbox.Algo
{
  public class RouletteWheelSelection<T>
  {
    private List<T> m_Members = new List<T>();
    private Random m_Random = new Random();

    private List<double> m_CumulativeWeight;

    public void Add(double weight, T member)
    {
      m_Members.Add(member);

      if (m_CumulativeWeight == null)
      {
        m_CumulativeWeight = new List<double> {weight};
      }
      else
      {
        m_CumulativeWeight.Add(m_CumulativeWeight.Last() + weight);
      }
    }

    public T Get()
    {
      var selection = m_Random.NextDouble()*m_CumulativeWeight.Last();
      for (var i = 0; i < m_CumulativeWeight.Count; i++)
      {
        if (m_CumulativeWeight[i] > selection) return m_Members[i];
      }

      throw new InvalidOperationException();
    }
  }
}
