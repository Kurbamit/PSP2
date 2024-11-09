// src/components/Base/Pagination.tsx
import React from 'react';
import { Pagination as BootstrapPagination, Form } from 'react-bootstrap';
import ScriptResources from "../../assets/resources/strings.ts";

interface PaginationProps {
    currentPage: number;
    totalPages: number;
    totalItems: number;
    pageSize: number;
    onPageChange: (page: number) => void;
    onPageSizeChange: (size: number) => void;
}

const Pagination: React.FC<PaginationProps> = ({
                                                   currentPage,
                                                   totalPages,
                                                   totalItems,
                                                   pageSize,
                                                   onPageChange,
                                                   onPageSizeChange,
                                               }) => {
    const handlePageClick = (page: number) => {
        if (page >= 1 && page <= totalPages) {
            onPageChange(page);
        }
    };

    const handlePageSizeChange = (event: React.ChangeEvent<HTMLSelectElement>) => {
        onPageSizeChange(parseInt(event.target.value, 10));
        onPageChange(1); // Reset to the first page when page size changes
    };

    return (
        <div>
            <div className="d-flex justify-content-between align-items-center mb-3">
                <span>{ScriptResources.TotalItems} {totalItems}</span>
                <Form.Group controlId="pageSizeSelect">
                    <Form.Label>{ScriptResources.ItemsPerPage}</Form.Label>
                    <Form.Control
                        as="select"
                        value={pageSize}
                        onChange={handlePageSizeChange}
                        style={{ width: '100px', display: 'inline-block', marginLeft: '10px' }}>
                        <option value={5}>5</option>
                        <option value={10}>10</option>
                        <option value={20}>20</option>
                        <option value={50}>50</option>
                    </Form.Control>
                </Form.Group>
            </div>

            <BootstrapPagination>
                <BootstrapPagination.Prev onClick={() => handlePageClick(currentPage - 1)} disabled={currentPage === 1} />
                {Array.from({ length: totalPages }, (_, i) => (
                    <BootstrapPagination.Item
                        key={i + 1}
                        active={i + 1 === currentPage}
                        onClick={() => handlePageClick(i + 1)}
                    >
                        {i + 1}
                    </BootstrapPagination.Item>
                ))}
                <BootstrapPagination.Next onClick={() => handlePageClick(currentPage + 1)} disabled={currentPage === totalPages} />
            </BootstrapPagination>
        </div>
    );
};

export default Pagination;
