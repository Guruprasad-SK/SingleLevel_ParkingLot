using ParkingLotEx;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace ParkingLotSimulation
{
    public class ParkingLot
    {
       // public int LevelId { get; set; }
        public List<ParkingSlot> listOfParkingSlots = new();
       public List<Ticket> ticketList = new();
        int _totalAvailabilitySlots { get; set; }
        int _ticketNo = 1;
        int UserTicketNo { get; set; }
        int _type { get; set; }
        public ParkingLot()
        {
           IntializationOfParkingSlot();
            ParkingLotOperation();
        }
        void IntializationOfParkingSlot()
        {
            Console.WriteLine("Enter number of slots for 2 wheeles vehicals");
            int _numberOfTwoWheelerSlots = ValidateInput(Console.ReadLine());

            Console.WriteLine("Enter number of slots for 4 wheeler vehical");
            int _numberOfFourWheelerSlots = ValidateInput(Console.ReadLine());

            Console.WriteLine("Enter number of  slots for Heavy  Vehicle");
            int _numberOfHeavyVehicalSlots = ValidateInput(Console.ReadLine());

            _totalAvailabilitySlots = (_numberOfTwoWheelerSlots + _numberOfFourWheelerSlots + _numberOfHeavyVehicalSlots);

            int j = 1;
            for (int i = 1; i <= _totalAvailabilitySlots; i++)
            {
                if (i >= 0 && i <= _numberOfTwoWheelerSlots)
                {
                    listOfParkingSlots.Add(new ParkingSlot { Id = j, SlotType = VehicleType.TwoWheelerVehicles });
                    j++;

                }
                else if (i > _numberOfTwoWheelerSlots && i <= _numberOfFourWheelerSlots + _numberOfTwoWheelerSlots)
                {
                    listOfParkingSlots.Add(new ParkingSlot { Id = j, SlotType = VehicleType.FourWheelerVehicles });
                    j++;

                }
                else if (i > _numberOfFourWheelerSlots && i <= _totalAvailabilitySlots)
                {
                    listOfParkingSlots.Add(new ParkingSlot { Id = j, SlotType = VehicleType.HeavyVehicles });
                    j++;
                }
            }

            DisplayStates();
        }

        void ParkingLotOperation()
        {
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("1.Park Vehicle\n2.Unpark Vehicle\n3 Exit");
                Console.WriteLine("Enter the choice");

                int _choice = ValidateInput(Console.ReadLine());
                switch (_choice)
                {
                    case 1:
                        Console.WriteLine();
                        Console.WriteLine("1.Two wheeler\n2fourwheeler\n3Heavy vehiclas");
                        ParkVehicalAndGenerateTicket();
                        break;

                    case 2:
                        Console.WriteLine("Enter Ticket no");

                        UnparkAndGenerateTicket();
                        break;
                    case 3:
                        Console.WriteLine("Thank you...have a nice day");
                        Console.ReadLine();
                        break;
                }
            }

        }

        int ValidateInput(string input)
        {
            int output;
            var isValid = int.TryParse(input, out output);
            if (isValid)
            {
                return output;
            }
            Console.WriteLine(" Enter Valid INPUT");
            var input2 = Console.ReadLine();
            return ValidateInput(input2);
        }
        void ParkVehicalAndGenerateTicket()
        {
            _type = ValidateInput(Console.ReadLine());

            switch (_type)
            {
                case 1:
                    Parking(VehicleType.TwoWheelerVehicles);
                    break;
                case 2:
                    Parking(VehicleType.FourWheelerVehicles);
                    break;
                case 3:
                    Parking(VehicleType.HeavyVehicles);
                    break;
                default:
                    Console.WriteLine("enter valid choice");
                    break;
            }
        }
        void DisplayStates()
        {
            foreach (var state in listOfParkingSlots)
            {
                var result = state.isAvailable ? "Available" : "NotAvialable";
                Console.WriteLine(" Slot {0} {1} {2},", state.Id, state.SlotType, result);
            }
        }

        void UnparkAndGenerateTicket()
        {
            UserTicketNo = ValidateInput(Console.ReadLine());
            var Tickets = ticketList.Find(i => i.TicketNo == UserTicketNo);
            if (Tickets == null)
            {
                Console.WriteLine("enter valid ticket number");
            }
            else
            {
                foreach (var slotdetails in listOfParkingSlots)
                {
                    if (slotdetails.Id == Tickets.SlotNo)
                    {
                        slotdetails.isAvailable = true;

                    }
                    Tickets.OutTime = DateTime.Now;
                }
                Console.WriteLine("unpark your vehicle");
                Console.WriteLine();
                Console.WriteLine("=> Ticket no=" + Tickets.TicketNo + "\n=>Slot Number = " + Tickets.SlotNo + "\n=>Vehical Number = " + Tickets.VehicalNo + "\n=>Intime = " + Tickets.InTime + "\n=>Outtime = " + Tickets.OutTime);
                Console.WriteLine();
                DisplayStates();
                ticketList.Remove(Tickets);
            }

        }
        void Parking(VehicleType type)
        {

            Console.WriteLine("Slots Availability in two wheeler parkingslot");
            var result1 = listOfParkingSlots.FindAll(x => x.SlotType == type);
            foreach (var p in result1)
            {
                var slotStates = p.isAvailable ? "Available" : "NotAvialable";
                Console.WriteLine(p.Id + " " + p.SlotType + " " + slotStates);
            }
            var result = listOfParkingSlots.Find(i => i.SlotType == type && i.isAvailable == true);
            if (result == null)
            {
                Console.WriteLine("No slots are available");
            }
            else
            {
                Console.WriteLine("Enter vehical number");
                var vehicleNo = Console.ReadLine();
                //slots availability
                foreach (var slot in listOfParkingSlots)
                {
                    if (slot.Id == result.Id)
                    {
                        slot.isAvailable = false;
                        Console.WriteLine("your vehical is parked");
                    }
                }

                ticketList.Add(new Ticket() { TicketNo = _ticketNo, InTime = DateTime.Now, VehicalNo = vehicleNo, SlotNo = result.Id });
                foreach (var ticketdetails in ticketList)
                {
                    if (ticketdetails.TicketNo == _ticketNo)
                    {
                        Console.WriteLine();
                        Console.WriteLine("=> Ticket no=" + ticketdetails.TicketNo + "\n=>Slot Number = " + ticketdetails.SlotNo + "\n=>Vehical Number = " + ticketdetails.VehicalNo + "\n=>Intime = " + ticketdetails.InTime);
                        Console.WriteLine();
                    }
                }
                _ticketNo++;
            }
            DisplayStates();
        }


    }
}

