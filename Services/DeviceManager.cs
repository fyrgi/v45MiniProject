
using System;
using System.Collections.Generic;
using System.Linq;
using v45MiniProject.Models;

namespace v45MiniProject.Services
{
    internal class DeviceManager
    {
        private readonly List<Device> _devices;
        private readonly List<Office> _offices;

        public DeviceManager(
            List<Device> devices,
            List<Office> offices)
        {
            _devices = devices;
            _offices = offices;
        }

        // Find office using its ID
        public Office? GetOfficeById(int officeId)
        {
            return _offices.FirstOrDefault(
                office => office.Id == officeId);
        }

        // Get all devices belonging to an office
        public List<Device> GetDevicesByOffice(int officeId)
        {
            return _devices
                .Where(device => device.OfficeId == officeId)
                .OrderBy(device => device.EndOfLifeDate)
                .ToList();
        }

        // Determine the device's end-of-life status
        public DeviceLifeStatus GetDeviceLifeStatus(
            Device device,
            DateTime today)
        {
            if (device.EndOfLifeDate <= today.AddMonths(3))
            {
                return DeviceLifeStatus.Red;
            }

            if (device.EndOfLifeDate <= today.AddMonths(6))
            {
                return DeviceLifeStatus.Yellow;
            }

            return DeviceLifeStatus.Normal;
        }

        // Get all devices, sorted by office and end-of-life
        public List<Device> GetAllDevicesSorted()
        {
            return _devices
                .OrderBy(device => device.OfficeId)
                .ThenBy(device => device.EndOfLifeDate)
                .ToList();
        }

        public List<Office> GetOffices()
        {
            return _offices.OrderBy(o => o.Id).ToList();
        }
    }
}
