﻿using System.ComponentModel.DataAnnotations;

namespace SmartdustApp.Business.Core.Model
{
    // ContactModel class represents a model for capturing contact information and messages from users.
    public class ContactModel
    {
        // The name of the person submitting the contact form.
        [Required]
        public string Name { get; set; }

        // The email address of the person submitting the contact form.
        [Required]
        public string Mail { get; set; }

        // The phone number of the person submitting the contact form.
        [Required]
        public int Phone { get; set; }

        // The subject of the contact message.
        [Required]
        public string Subject { get; set; }

        // The address of the person submitting the contact form.
        [Required]
        public string Address { get; set; }

        // The message or content of the contact form submission.
        [Required]
        public string Message { get; set; }
    }
}
