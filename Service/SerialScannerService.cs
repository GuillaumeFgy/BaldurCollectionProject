using System.IO.Ports;

namespace MyApp.Service;

public class SerialScannerService
{
    public event EventHandler<string>? DataReceived;

    private SerialPort? _serialPort;

    public void Open(string portName = "COM3", int baudRate = 9600)
    {
        _serialPort = new SerialPort(portName, baudRate, Parity.None, 8, StopBits.One);
        _serialPort.DataReceived += OnDataReceived;
        _serialPort.Open();
    }

    public void Close()
    {
        if (_serialPort != null && _serialPort.IsOpen)
        {
            _serialPort.DataReceived -= OnDataReceived;
            _serialPort.Close();
        }
    }

    private void OnDataReceived(object sender, SerialDataReceivedEventArgs e)
    {
        var sp = (SerialPort)sender;
        string data = sp.ReadExisting().Trim();
        if (!string.IsNullOrWhiteSpace(data))
            DataReceived?.Invoke(this, data);
    }
}
