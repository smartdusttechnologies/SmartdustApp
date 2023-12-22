// DeleteComponent.test.js
import React from 'react';
import { render, fireEvent, screen } from '@testing-library/react';
import DeleteComponent from '../DeleteComponent';

test('renders DeleteComponent with delete button', () => {
    const onDeleteMock = jest.fn();
    render(<DeleteComponent onDelete={onDeleteMock} id={1} />);

    const deleteButton = screen.getByRole('button', { name: /Delete/i });
    expect(deleteButton).toBeInTheDocument();

    fireEvent.click(deleteButton);
    const confirmDeleteButton = screen.getByRole('button', { name: /Delete/i });
    const cancelButton = screen.getByRole('button', { name: /Cancel/i });
    expect(confirmDeleteButton).toBeInTheDocument();
    expect(cancelButton).toBeInTheDocument();

    fireEvent.click(confirmDeleteButton);
    expect(onDeleteMock).toHaveBeenCalledWith(1);
});

test('closes the dialog when Cancel button is clicked', () => {
    const onDeleteMock = jest.fn();
    render(<DeleteComponent onDelete={onDeleteMock} id={1} />);

    const deleteButton = screen.getByRole('button', { name: /Delete/i });
    fireEvent.click(deleteButton);

    const cancelButton = screen.getByRole('button', { name: /Cancel/i });
    expect(cancelButton).toBeInTheDocument();

    fireEvent.click(cancelButton);
    const confirmDeleteButton = screen.queryByRole('button', { name: /Delete/i });
    expect(confirmDeleteButton).not.toBeInTheDocument();
});
