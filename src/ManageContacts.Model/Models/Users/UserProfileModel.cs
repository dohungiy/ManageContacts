﻿using ManageContacts.Model.Models.Roles;
using Newtonsoft.Json;

namespace ManageContacts.Model.Models.Users;

public class UserProfileModel
{
    [JsonProperty(PropertyName = "id")]
    public Guid Id { get; set; }
    
    [JsonProperty(PropertyName = "user_name")]
    public string UserName { get; set; }
    
    [JsonProperty(PropertyName = "email")]
    public string Email { get; set; }
    
    [JsonProperty(PropertyName = "first_name")]
    public string FirstName { get; set; }
    
    [JsonProperty(PropertyName = "last_name")]
    public string LastName { get; set; }
    
    [JsonProperty(PropertyName = "phone_number")]
    public string PhoneNumber { get; set; }
    
    [JsonProperty(PropertyName = "avatar")]
    public string Avatar { get; set; }
    
    [JsonProperty(PropertyName = "roles")]
    public ICollection<RoleModel> Roles { get; set; }
}