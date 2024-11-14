# Gym Class Booking Application

## Overview

This Gym Class Booking Application allows users to book and manage their gym classes, view available classes, and see their bookings.

---

## Features

- **User Authentication**: Users can register, log in, and manage their accounts. User profile data (first name, last name) is displayed after successful login.
- **Gym Class Management**: Admin users can create, edit, and delete gym classes. They can also view a list of all available gym classes.
- **Booking System**: Users can book available gym classes, and if they have already booked a class, they can "unbook" it. The booking status is dynamically reflected in the UI.
- **User-Specific Class Views**: Logged-in users can view only their own booked classes, and only those that have not yet started. Non-authenticated users can view all available classes.

---

## Key Features & Functionalities

### User Registration & Authentication

- Users can create accounts using their email and password.
- Upon registration, a claim is added for the user's full name (first and last names), which is displayed in the navigation bar after login instead of the email.
- Users can log in and manage their account settings, including phone number and username.

### Admin Functionality

- Admins can create, edit, and delete gym classes. They have access to all features, including a "Manage" section to perform CRUD operations on gym classes.
- Admins can view all classes, including those that have been booked.

### Booking Classes

- Users can book a class by clicking the "Book" button. If the user is already booked for the class, the button will show "Unbook" instead.
- Classes that have started cannot be booked or unbooked by users.
- Users can only see their own classes that are upcoming (not yet started).

### Class Management

- Admin users can set the name, start time, duration, and description of each gym class.
- Gym classes are displayed in a table format, with each class showing its name, start time, duration, and description.
- For each class, users can see whether they’ve already booked it.

---

## Database Structure

### Gym Classes

- **Name**: Name of the gym class (e.g., "Yoga", "Zumba").
- **StartTime**: DateTime when the class begins.
- **Duration**: The duration of the class in `TimeSpan` format.
- **EndTime**: The computed end time, based on `StartTime` and `Duration`.
- **Description**: A brief description of the gym class.

### Bookings (Many-to-Many Relationship)

- **ApplicationUserGymClass**: The many-to-many relationship between users and gym classes. Each record represents a user’s booking for a gym class.

### ApplicationUser

- Users are identified by their email, and upon registration, a full name claim is stored for displaying in the UI.

---

## Validation

- **Gym Class Validation**:
  - **Name**: Must be provided and cannot exceed 100 characters.
  - **Start Time**: Must be a future date.
  - **Duration**: Must be between 10 minutes and 10 hours.
  - **Description**: Must be 500 characters or less.
  
- **Custom Validation**:
  - `FutureDate` ensures that the `StartTime` of a gym class is in the future.

---

## Documentation requirements

[Övning 13 - Användarhantering för passbokning.pdf](https://github.com/user-attachments/files/17754467/Ovning.13.-.Anvandarhantering.for.passbokning.pdf)
