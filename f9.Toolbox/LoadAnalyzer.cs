using System.Diagnostics;

namespace f9.Toolbox
{
  public class LoadAnalyzer
  {
    private readonly Stopwatch m_DurationStopwatch = new Stopwatch();
    private readonly Stopwatch m_PeriodStopwatch = new Stopwatch();

    private bool m_IsFirstRun = true;

    /// <summary>
    /// Gets the period  [ms].
    /// </summary>
    /// <value>
    /// The period  [ms].
    /// </value>
    public float Period { get; private set; }
    /// <summary>
    /// Gets the duration [ms].
    /// </summary>
    /// <value>
    /// The duration [ms].
    /// </value>
    public float Duration { get; private set; }

    /// <summary>
    /// Gets or sets the last duration [ms].
    /// </summary>
    /// <value>The last duration [ms].</value>
    public float LastDuration { get; private set; }

    /// <summary>
    /// Gets the maximum duration  [ms].
    /// </summary>
    /// <value>
    /// The maximum duration  [ms].
    /// </value>
    public float MaxDuration { get; private set; }

    /// <summary>
    /// Gets the load [%].
    /// </summary>
    /// <value>
    /// The load  [%].
    /// </value>
    public float Load { get; private set; }

    public float SmoothingFactor { get; set; }

    public LoadAnalyzer()
    {
      SmoothingFactor = 0.9f;
    }

    public void StartMeasuring()
    {
      if(!m_IsFirstRun)
      {
        Period = SmoothingFactor*Period + (1 - SmoothingFactor)*m_PeriodStopwatch.ElapsedMilliseconds;
      }
      else
      {
        m_IsFirstRun = false;
      }
      m_PeriodStopwatch.Reset();
      m_PeriodStopwatch.Start();

      m_DurationStopwatch.Reset();
      m_DurationStopwatch.Start();
    }

    /// <summary>
    /// Checks the end of process.
    /// </summary>
    /// <returns>True id the maximum duration was exceeded</returns>
    public void StopMeasuring()
    {
      m_DurationStopwatch.Stop();

      LastDuration = m_DurationStopwatch.ElapsedMilliseconds;

      if (LastDuration > MaxDuration)
      {
        MaxDuration = LastDuration;
      }

      Duration = SmoothingFactor * Duration + (1 - SmoothingFactor) * LastDuration;

      if (!m_IsFirstRun)
      {
        Load = 100 * Duration/Period;
      }
      else
      {
        m_IsFirstRun = false;
      }
    }

    public void ResetMaxDuration()
    {
      MaxDuration = 0;
    }

    public void Reset()
    {
      MaxDuration = 0;
      Load = 0;
      Duration = 0;
      Period = 0;
      LastDuration = 0;
    }
  }
}
