using AutoMapper;
using Domain.Devices.Domain;
using Domain.Devices.Models.Company;
using Domain.Devices.Models.Partners;

namespace Application.Concentric
{
    public class DeviceMapper : Profile
    {
        public DeviceMapper()
        {
            CreateMap<PartnerData, IoTData>()
                .ForMember(dest => dest.CompanyId, opt => opt.MapFrom(src => src.PartnerId))
                .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.PartnerName))
                .ForMember(dest => dest.DeviceId, opt => opt.MapFrom(src => src.Trackers.Select(d => d.Id).FirstOrDefault()))
                .ForMember(dest => dest.DeviceName, opt => opt.MapFrom(src => src.Trackers.Select(d => d.Model).FirstOrDefault()))
                .ForMember(dest => dest.FirstReadingDtm, opt => opt.MapFrom(src => src.Trackers.SelectMany(t => t.Sensors).SelectMany(s => s.Crumbs).Min(c => c.CreatedDtm)))
                .ForMember(dest => dest.LastReadingDtm, opt => opt.MapFrom(src => src.Trackers.SelectMany(t => t.Sensors).SelectMany(s => s.Crumbs).Max(c => c.CreatedDtm)))
                .ForMember(dest => dest.TemperatureCount, opt => opt.MapFrom(src => src.Trackers.SelectMany(t => t.Sensors).Count(s => s.Name == "Temperature")))
                .ForMember(dest => dest.AverageTemperature, opt => opt.MapFrom(src => src.Trackers.SelectMany(t => t.Sensors).Where(s => s.Name == "Temperature").Average(s => s.Crumbs.Average(c => c.Value))))
                .ForMember(dest => dest.HumidityCount, opt => opt.MapFrom(src => src.Trackers.SelectMany(t => t.Sensors).Count(s => s.Name == "Humidty")))
                .ForMember(dest => dest.AverageHumidity, opt => opt.MapFrom(src => src.Trackers.SelectMany(t => t.Sensors).Where(s => s.Name == "Humidty").Average(s => s.Crumbs.Average(c => c.Value))));

            CreateMap<CompanyData, IoTData>()
                .ForMember(dest => dest.CompanyId, opt => opt.MapFrom(src => src.CompanyId))
                .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.Company))
                .ForMember(dest => dest.DeviceId, opt => opt.MapFrom(src => src.Devices.FirstOrDefault().DeviceID))
                .ForMember(dest => dest.DeviceName, opt => opt.MapFrom(src => src.Devices.FirstOrDefault().Name))
                .ForMember(dest => dest.FirstReadingDtm, opt => opt.MapFrom(src => src.Devices.SelectMany(d => d.SensorData).Min(sd => DateTime.Parse(sd.DateTime))))
                .ForMember(dest => dest.LastReadingDtm, opt => opt.MapFrom(src => src.Devices.SelectMany(d => d.SensorData).Max(sd => DateTime.Parse(sd.DateTime))))
                .ForMember(dest => dest.TemperatureCount, opt => opt.MapFrom(src => src.Devices.SelectMany(d => d.SensorData).Count(sd => sd.SensorType == "TEMP")))
                .ForMember(dest => dest.AverageTemperature, opt => opt.MapFrom(src => src.Devices.SelectMany(d => d.SensorData).Where(sd => sd.SensorType == "TEMP").Average(sd => sd.Value)))
                .ForMember(dest => dest.HumidityCount, opt => opt.MapFrom(src => src.Devices.SelectMany(d => d.SensorData).Count(sd => sd.SensorType == "HUM")))
                .ForMember(dest => dest.AverageHumidity, opt => opt.MapFrom(src => src.Devices.SelectMany(d => d.SensorData).Where(sd => sd.SensorType == "HUM").Average(sd => sd.Value)));
        }
    }
}
