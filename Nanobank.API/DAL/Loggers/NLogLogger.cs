using System;
using NLog;

namespace Nanobank.API.DAL.Loggers
{
  public class NLogLogger : ILogger
  {
    private readonly bool _isDebugLevel;
    private static readonly Logger _logger;

    static NLogLogger()
    {
      _logger = LogManager.GetCurrentClassLogger();
    }

    private NLogLogger(bool isDebugLevel)
    {
      _isDebugLevel = isDebugLevel;
    }

    public static NLogLogger Create(bool isDebugLevel)
    {
      return new NLogLogger(isDebugLevel);
    }

    public void Debug(string message, Exception exception)
    {
      if (!_isDebugLevel)
      {
        return;
      }
      
      if (exception == null)
      {
        _logger.Debug(message);
      }
      else
      {
        _logger.Debug(exception, message);
      }
    }

    public void Error(string message, Exception exception)
    {
      if (!_isDebugLevel)
      {
        return;
      }

      if (exception == null)
      {
        _logger.Error(message);
      }
      else
      {
        _logger.Error(exception, message);
      }
    }

    public void Fatal(string message, Exception exception)
    {
      if (!_isDebugLevel)
      {
        return;
      }

      if (exception == null)
      {
        _logger.Fatal(message);
      }
      else
      {
        _logger.Fatal(exception, message);
      }
    }

    public void Info(string message, Exception exception)
    {
      if (!_isDebugLevel)
      {
        return;
      }

      if (exception == null)
      {
        _logger.Info(message);
      }
      else
      {
        _logger.Info(exception, message);
      }
    }

    public void Trace(string message, Exception exception)
    {
      if (!_isDebugLevel)
      {
        return;
      }

      if (exception == null)
      {
        _logger.Trace(message);
      }
      else
      {
        _logger.Trace(exception, message);
      }
    }

    public void Warn(string message, Exception exception)
    {
      if (!_isDebugLevel)
      {
        return;
      }

      if (exception == null)
      {
        _logger.Warn(message);
      }
      else
      {
        _logger.Warn(exception, message);
      }
    }

    public void Dispose()
    {
    }
  }
}