﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34209
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------



[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
[System.ServiceModel.ServiceContractAttribute(ConfigurationName="KinectServer2")]
public interface KinectServer2
{
    
    [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/KinectServer2/LatestDepthImage", ReplyAction="http://tempuri.org/KinectServer2/LatestDepthImageResponse")]
    byte[] LatestDepthImage();
    
    [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/KinectServer2/LatestDepthImage", ReplyAction="http://tempuri.org/KinectServer2/LatestDepthImageResponse")]
    System.Threading.Tasks.Task<byte[]> LatestDepthImageAsync();
    
    [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/KinectServer2/LatestYUVImage", ReplyAction="http://tempuri.org/KinectServer2/LatestYUVImageResponse")]
    byte[] LatestYUVImage();
    
    [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/KinectServer2/LatestYUVImage", ReplyAction="http://tempuri.org/KinectServer2/LatestYUVImageResponse")]
    System.Threading.Tasks.Task<byte[]> LatestYUVImageAsync();
    
    [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/KinectServer2/LatestRGBImage", ReplyAction="http://tempuri.org/KinectServer2/LatestRGBImageResponse")]
    byte[] LatestRGBImage();
    
    [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/KinectServer2/LatestRGBImage", ReplyAction="http://tempuri.org/KinectServer2/LatestRGBImageResponse")]
    System.Threading.Tasks.Task<byte[]> LatestRGBImageAsync();
    
    [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/KinectServer2/LatestJPEGImage", ReplyAction="http://tempuri.org/KinectServer2/LatestJPEGImageResponse")]
    byte[] LatestJPEGImage();
    
    [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/KinectServer2/LatestJPEGImage", ReplyAction="http://tempuri.org/KinectServer2/LatestJPEGImageResponse")]
    System.Threading.Tasks.Task<byte[]> LatestJPEGImageAsync();
    
    [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/KinectServer2/LastColorGain", ReplyAction="http://tempuri.org/KinectServer2/LastColorGainResponse")]
    float LastColorGain();
    
    [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/KinectServer2/LastColorGain", ReplyAction="http://tempuri.org/KinectServer2/LastColorGainResponse")]
    System.Threading.Tasks.Task<float> LastColorGainAsync();
    
    [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/KinectServer2/LastColorExposureTimeTicks", ReplyAction="http://tempuri.org/KinectServer2/LastColorExposureTimeTicksResponse")]
    long LastColorExposureTimeTicks();
    
    [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/KinectServer2/LastColorExposureTimeTicks", ReplyAction="http://tempuri.org/KinectServer2/LastColorExposureTimeTicksResponse")]
    System.Threading.Tasks.Task<long> LastColorExposureTimeTicksAsync();
    
    [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/KinectServer2/GetCalibration", ReplyAction="http://tempuri.org/KinectServer2/GetCalibrationResponse")]
    RoomAliveToolkit.Kinect2Calibration GetCalibration();
    
    [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/KinectServer2/GetCalibration", ReplyAction="http://tempuri.org/KinectServer2/GetCalibrationResponse")]
    System.Threading.Tasks.Task<RoomAliveToolkit.Kinect2Calibration> GetCalibrationAsync();
}

[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
public interface KinectServer2Channel : KinectServer2, System.ServiceModel.IClientChannel
{
}

[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
public partial class KinectServer2Client : System.ServiceModel.ClientBase<KinectServer2>, KinectServer2
{
    
    public KinectServer2Client()
    {
    }
    
    public KinectServer2Client(string endpointConfigurationName) : 
            base(endpointConfigurationName)
    {
    }
    
    public KinectServer2Client(string endpointConfigurationName, string remoteAddress) : 
            base(endpointConfigurationName, remoteAddress)
    {
    }
    
    public KinectServer2Client(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
            base(endpointConfigurationName, remoteAddress)
    {
    }
    
    public KinectServer2Client(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
            base(binding, remoteAddress)
    {
    }
    
    public byte[] LatestDepthImage()
    {
        return base.Channel.LatestDepthImage();
    }
    
    public System.Threading.Tasks.Task<byte[]> LatestDepthImageAsync()
    {
        return base.Channel.LatestDepthImageAsync();
    }
    
    public byte[] LatestYUVImage()
    {
        return base.Channel.LatestYUVImage();
    }
    
    public System.Threading.Tasks.Task<byte[]> LatestYUVImageAsync()
    {
        return base.Channel.LatestYUVImageAsync();
    }
    
    public byte[] LatestRGBImage()
    {
        return base.Channel.LatestRGBImage();
    }
    
    public System.Threading.Tasks.Task<byte[]> LatestRGBImageAsync()
    {
        return base.Channel.LatestRGBImageAsync();
    }
    
    public byte[] LatestJPEGImage()
    {
        return base.Channel.LatestJPEGImage();
    }
    
    public System.Threading.Tasks.Task<byte[]> LatestJPEGImageAsync()
    {
        return base.Channel.LatestJPEGImageAsync();
    }
    
    public float LastColorGain()
    {
        return base.Channel.LastColorGain();
    }
    
    public System.Threading.Tasks.Task<float> LastColorGainAsync()
    {
        return base.Channel.LastColorGainAsync();
    }
    
    public long LastColorExposureTimeTicks()
    {
        return base.Channel.LastColorExposureTimeTicks();
    }
    
    public System.Threading.Tasks.Task<long> LastColorExposureTimeTicksAsync()
    {
        return base.Channel.LastColorExposureTimeTicksAsync();
    }
    
    public RoomAliveToolkit.Kinect2Calibration GetCalibration()
    {
        return base.Channel.GetCalibration();
    }
    
    public System.Threading.Tasks.Task<RoomAliveToolkit.Kinect2Calibration> GetCalibrationAsync()
    {
        return base.Channel.GetCalibrationAsync();
    }
}
