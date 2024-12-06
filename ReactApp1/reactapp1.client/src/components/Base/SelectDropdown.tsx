import React from "react";
import AsyncSelect from "react-select/async";
import { API_BASE_URL } from "../../../config";
import Cookies from "js-cookie";

interface Option {
    id: number;
    name: string;
}

interface SelectDropdownProps {
    endpoint: string;
    onSelect?: (selectedOption: Option | null) => void;
}

const SelectDropdown: React.FC<SelectDropdownProps> = ({ endpoint, onSelect }) => {
    const token = Cookies.get("authToken");

    const loadOptions = async (inputValue: string) => {
        try {
            const queryParam = inputValue ? `?search=${encodeURIComponent(inputValue)}` : "";
            const response = await fetch(`${API_BASE_URL}${endpoint}${queryParam}`, {
                method: "GET",
                headers: {
                    Authorization: `Bearer ${token}`,
                },
            });

            if (!response.ok) {
                throw new Error("Failed to fetch data");
            }

            const data: Option[] = await response.json();

            return data.map((option) => ({
                value: option.id,
                label: option.name,
            }));
        } catch (err) {
            console.error(err);
            return [];
        }
    };

    const handleSelectChange = (selectedOption: { value: number; label: string } | null) => {
        if (onSelect) {
            const selected = selectedOption
                ? { id: selectedOption.value, name: selectedOption.label }
                : null;
            onSelect(selected);
        }
    };

    return (
        <div>
            <AsyncSelect
                cacheOptions
                defaultOptions
                loadOptions={loadOptions}
                onChange={handleSelectChange}
                placeholder="Search and select..."
                styles={{
                    control: (base) => ({
                        ...base,
                        minHeight: "40px",
                    }),
                }}
            />
        </div>
    );
};

export default SelectDropdown;
