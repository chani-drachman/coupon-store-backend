# CouponShop – Full Stack Coupon Platform

## Overview
CouponShop is a full-stack web application for managing and purchasing digital coupons for local businesses.  
The platform supports multiple user roles and real-world workflows, including authentication, authorization, approval processes, and controlled content publishing.  

The project was developed as a portfolio-level application to demonstrate full-stack development skills using modern technologies.

---

## Tech Stack

### Frontend
- React
- Redux (Global State Management)
- Material UI (MUI)
- Axios

### Backend
- ASP.NET Web API (.NET)
- JWT Authentication
- Role-Based Authorization
- SQL Server

---

## System Architecture
The project is divided into two separate repositories:
- **Frontend Repository** – React application  
- **Backend Repository** – ASP.NET Web API  

This separation reflects a real-world production setup and ensures clear separation of concerns.

---

## Authentication & Security
- Authentication implemented using JWT  
- Role-based authorization for:
  - Customer
  - Business Owner
  - Administrator  
- Passwords are stored encrypted in the database  
- Role-based authorization is enforced on backend API endpoints, while frontend access is controlled through role-based UI rendering.

---

## User Roles & Features

### Customers
- Register and log in to a personal account
- Browse available coupons
- Filter coupons by category
- Purchase coupons (simulated payment flow)
- View order history
- View personal coupon codes
- Track coupon code status:
  - Active
  - Redeemed

### Business Owners
- Receive email onboarding after admin approval
- Set a permanent password via a secure, token-based link
- Validate coupon codes
- Mark coupons as redeemed

### Administrators
- View and manage:
  - Coupons
  - Orders
  - Join requests  
- Review business & coupon join requests
- Request statuses:
  - PENDING
  - APPROVED  
- Approval workflow:
  - Automatically creates a new business and coupon
  - Sends an email to the business owner with a temporary token  
- Control coupon visibility:
  - Coupons are created as Inactive
  - Inactive coupons are visible only to administrators
  - Admins can preview coupons before publishing
  - Activate/deactivate coupons with one action

---

## Coupon & Request Lifecycle

### Coupon (Product)
- Active
- Inactive

### Coupon Code (Per Customer)
- Active
- Redeemed

### Join Request
- Pending
- Approved

---

## Categories & Filtering
- Coupons are associated with categories  
- Category-based filtering available for customers  
- Filtering handled on both frontend and backend

---

## State Management
- Global application state managed with Redux  
- Centralized handling of authentication, coupons, orders, and user data  
- Async API communication handled via middleware

---

## Database
- SQL Server  
- Relational data model including:
  - Users & Roles
  - Businesses
  - Coupons
  - Coupon Codes
  - Orders
  - Categories
  - Join Requests

---

## Known Limitations & Future Improvements
- Admin interface currently does not allow viewing full customer or business details; this will be implemented in future updates  
- Payment system is currently simulated (no real payment gateway)  
- Error handling requires further improvement  
- No automated testing implemented yet  
- Deployment to cloud environment is planned  

These points were intentionally left as future enhancements to allow continued iteration and improvement.

---

## Purpose
This project was created as a professional portfolio project to demonstrate:
- Full-stack development
- Secure authentication and authorization
- Real-world business workflows
- Scalable system architecture
- Clean separation between frontend and backend

---

## Repositories
- **Frontend:** React + Redux application  
  [CouponShop.Client](https://github.com/chani-drachman/coupon-store)
- **Backend:** ASP.NET Web API  

Each repository contains setup and run instructions.





