import React, { useEffect, useState } from 'react';
import axios from 'axios';
import Cookies from 'js-cookie';
import { useNavigate } from 'react-router-dom';
import ScriptResources from "../../../assets/resources/strings.ts";
import moment from 'moment';

interface Reservation {
    customerPhoneNumber: string;
    establishmentAddress: string;
    establishmentAddressId: number;
    date?: string;
    workingHours?: string;
}

const ReservationDetail: React.FC = () => {
    const [reservation, setReservation] = useState<Reservation | null>(null);
    const [isEditing, setIsEditing] = useState(true);
    const [customerPhoneNumber, setCustomerPhoneNumber] = useState('');
    const [establishmentAddress, setEstablishmentAddress] = useState('');
    const [establishmentAddressId, setEstablishmentAddressId] = useState<number | null>(null);
    const [establishmentId, setEstablishmentId] = useState<number | null>(null);
    const [services, setServices] = useState<any[]>([]);
    const [selectedServiceId, setSelectedServiceId] = useState<number | null>(null);
    const [serviceLength, setServiceLength] = useState<string | null>(null);
    const [selectedDate, setSelectedDate] = useState('');
    const [timeSlots, setTimeSlots] = useState<string[]>([]);
    const [currentlySelectedTimeSlot, setCurrentlySelectedTimeSlot] = useState<string | null>(null);
    const [workingHours, setWorkingHours] = useState('');
    const token = Cookies.get('authToken');
    const navigate = useNavigate();
    const [error, setError] = useState('');

    useEffect(() => {
        const fetchEstablishmentAddress = async () => {
            try {
                const employeeResponse = await axios.get(`http://localhost:5114/api/employees/current`, {
                    headers: { Authorization: `Bearer ${token}` },
                });

                const establishmentId = employeeResponse.data.establishmentId;
                setEstablishmentId(establishmentId);

                const establishmentResponse = await axios.get(`http://localhost:5114/api/establishments/${establishmentId}`, {
                    headers: { Authorization: `Bearer ${token}` },
                });

                const establishmentData = establishmentResponse.data;

                const address = `${establishmentData.street} ${establishmentData.streetNumber}`;

                setEstablishmentAddress(address);
                setEstablishmentAddressId(establishmentData.establishmentAddressId);

                setReservation({
                    customerPhoneNumber: '',
                    establishmentAddress: address,
                    establishmentAddressId: establishmentData.establishmentAddressId,
                });
            } catch (error) {
                console.error(ScriptResources.ErrorFetchingEstablishmentDetails, error);
            }
        };

        fetchEstablishmentAddress();
    }, [token]);

    useEffect(() => {
        const fetchServices = async () => {
            if (!establishmentId) return;

            try {
                const servicesResponse = await axios.get(`http://localhost:5114/api/services?pageNumber=1&pageSize=100`, {
                    headers: { Authorization: `Bearer ${token}` },
                });

                const paginatedData = servicesResponse.data;
                const servicesData = paginatedData.items;

                const filteredServices = servicesData.filter(
                    (service: any) => service.establishmentId === establishmentId
                );

                setServices(filteredServices);
            } catch (error) {
                console.error(ScriptResources.ErrorFetchingServices, error);
                setServices([]);
            }
        };

        fetchServices();
    }, [establishmentId, token]);

    useEffect(() => {
        const fetchWorkingHours = async () => {
            if (!selectedDate || !establishmentId) return;

            try {
                const date = new Date(selectedDate);
                const americanDayOfWeek = date.getDay(); // JavaScript's getDay(): 0=Sunday, 6=Saturday. Its stupid.

                const numberdayweek = [7, 1, 2, 3, 4, 5, 6];
                const normalPersonDayOfWeek = numberdayweek[americanDayOfWeek];

                const workingHoursResponse = await axios.get(`http://localhost:5114/api/workinghours?pageNumber=1&pageSize=100`, {
                    headers: { Authorization: `Bearer ${token}` },
                });

                const paginatedData = workingHoursResponse.data;
                const workingHoursData = paginatedData.items;

                const matchedWorkingHour = workingHoursData.find((wh: any) =>
                    wh.establishmentAddressId === establishmentAddressId && wh.dayOfWeek === normalPersonDayOfWeek
                );

                setWorkingHours(matchedWorkingHour ? `${matchedWorkingHour.startTime} - ${matchedWorkingHour.endTime}` : ScriptResources.NoWorkingHours);
            } catch (error) {
                console.error(ScriptResources.ErrorFetchingWorkingHours, error);
            }
        };

        fetchWorkingHours();
    }, [selectedDate, establishmentId, token]);

    const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const { name, value } = e.target;
        if (name === 'customerPhoneNumber') {
            setCustomerPhoneNumber(value);
        } else if (name === 'date') {
            setSelectedDate(value);
        }
    };

    useEffect(() => {
        if (!workingHours || !serviceLength || !selectedDate || !selectedServiceId) return;

        const [startTime, endTime] = workingHours.split(' - ').map(time => moment(time, "HH:mm:ss"));
        const serviceDuration = moment.duration(serviceLength);

        if (!startTime || !endTime || !serviceDuration) {
            console.error("Invalid working hours or service length format.");
            return;
        }

        const slots = [];
        const currentTime = startTime.clone();

        slots.push(currentTime.clone().format("HH:mm:ss"));

        while (currentTime.add(serviceDuration).isBefore(endTime)) {
            slots.push(currentTime.clone().format("HH:mm:ss"));
        }

        console.log("Time Slots:", slots);
        setTimeSlots(slots);
    }, [workingHours, serviceLength, selectedDate, selectedServiceId]);

    const handleServiceChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
        const serviceId = parseInt(e.target.value, 10);
        setSelectedServiceId(serviceId);

        const selectedService = services.find(service => service.serviceId === serviceId);

        if (selectedService) {
            const serviceLength = selectedService.serviceLength;
            console.log("Service Length:", serviceLength);
            setServiceLength(serviceLength);
        } else {
            console.warn("Selected service not found in the available services list.");
        }

        console.log("Selected Service ID:", serviceId);
    };

    const handleTimeSlotChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
        const timeSlot = e.target.value;
        setCurrentlySelectedTimeSlot(timeSlot);
        console.log("Selected Time Slot:", timeSlot);
    };

    const validateForm = () => {
        if (!customerPhoneNumber) {
            setError(ScriptResources.CustomerPhoneNumberRequired);
            return false;
        }
        if (!selectedDate) {
            setError(ScriptResources.DateRequired);
            return false;
        }
        if (!selectedServiceId) {
            setError(ScriptResources.ServiceRequired);
            return false;
        }
        return true;
    };

    const handleSave = async () => {
        if (validateForm()) {
            try {
                const startTimeMoment = moment(`${selectedDate}T${currentlySelectedTimeSlot}`);
                const serviceDuration = moment.duration(serviceLength);

                const endTimeMoment = startTimeMoment.clone().add(serviceDuration);

                const startTime = startTimeMoment.toISOString();
                const endTime = endTimeMoment.toISOString();

                const payload = {
                    startTime,
                    endTime,
                    establishmentId,
                    establishmentAddressId,
                    serviceId: selectedServiceId,
                    customerPhoneNumber,
                };

                console.log("Payload:", payload);

                await axios.post(`http://localhost:5114/api/reservations`, payload, {
                    headers: { Authorization: `Bearer ${token}` },
                });

                navigate('/reservations');
            } catch (error) {
                console.error(ScriptResources.ErrorSavingItem, error);
                setError(ScriptResources.ErrorSavingItem);
            }
        }
    };


    const handleBackToList = () => {
        navigate('/reservations');
    };

    return (
        <div className="container-fluid">
            <h2 className="mb-4">{ScriptResources.CreateNewReservation}</h2>
            {reservation ? (
                <div className="card">
                    <div className="card-body">
                        <h5 className="card-title">{ScriptResources.ReservationInformation}</h5>
                        <div className="row">
                            <div className="col-12">
                                <ul className="list-group list-group-flush">
                                    <li className="list-group-item">
                                        <strong>{ScriptResources.CustomerPhoneNumber}</strong>{' '}
                                        <input
                                            type="text"
                                            name="customerPhoneNumber"
                                            value={customerPhoneNumber}
                                            onChange={handleInputChange}
                                            className="form-control"
                                            disabled={!isEditing}
                                        />
                                    </li>
                                    <li className="list-group-item">
                                        <strong>{ScriptResources.EstablishmentAddress}</strong>{' '}
                                        <input
                                            type="text"
                                            value={establishmentAddress}
                                            className="form-control"
                                            disabled={true}
                                        />
                                    </li>
                                    <li className="list-group-item">
                                        <strong>{ScriptResources.Service}</strong>{' '}
                                        <select
                                            value={selectedServiceId || ''}
                                            onChange={handleServiceChange}
                                            className="form-control"
                                        >
                                            <option value="">{ScriptResources.SelectService}</option>
                                            {services.map((service) => (
                                                <option key={service.serviceId} value={service.serviceId}>
                                                    {service.name}
                                                </option>
                                            ))}
                                        </select>
                                    </li>
                                    <li className="list-group-item">
                                        <strong>{ScriptResources.Date}</strong>{' '}
                                        <input
                                            type="date"
                                            name="date"
                                            value={selectedDate}
                                            onChange={handleInputChange}
                                            className="form-control"
                                            disabled={!isEditing}
                                        />
                                    </li>
                                    <li className="list-group-item">
                                        <strong>{ScriptResources.WorkingHours}</strong>{' '}
                                        <input
                                            type="text"
                                            value={workingHours}
                                            className="form-control"
                                            disabled={true}
                                        />
                                    </li>
                                    <li className="list-group-item">
                                        <strong>{ScriptResources.TimeSlot}</strong>{' '}
                                        <select
                                            value={currentlySelectedTimeSlot || ''}
                                            onChange={handleTimeSlotChange}
                                            className="form-control"
                                        >
                                            <option value="">{ScriptResources.SelectTimeSlot}</option>
                                            {timeSlots.map((slot, index) => (
                                                <option key={index} value={slot}>
                                                    {slot}
                                                </option>
                                            ))}
                                        </select>
                                    </li>
                                </ul>
                            </div>
                        </div>
                        <div className="mt-3">
                            <button className="btn btn-success me-2" onClick={handleSave}>
                                {ScriptResources.Save}
                            </button>
                            <button className="btn btn-secondary" onClick={handleBackToList}>
                                {ScriptResources.Cancel}
                            </button>
                        </div>
                    </div>
                </div>
            ) : (
                <p>{ScriptResources.Loading}</p>
            )}
        </div>
    );
};

export default ReservationDetail;
