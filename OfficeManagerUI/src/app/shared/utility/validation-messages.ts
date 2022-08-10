export const VALIDATION_MESSAGE = {
  firstName: {
    required: 'First name is required.',
    maxLength: 'First name must not exceed limit of 200 characters.'
  },
  lastName: {
      required: 'Last name is required.',
      maxLength: 'Last name must not exceed limit of 200 characters.'
  },
  email:{
      required: 'Email is required.',
      email: 'Please enter a proper email.',
      maxLength: 'Email must not exceed limit of 200 characters.'
  },
  personalEmail:{
      required: 'Personal Email is required.',
      email: 'Please enter a proper email.',
      maxLength: 'Personal Email must not exceed limit of 200 characters.'
  },
  phoneNumber:{
      required: 'Phone number is required.',
      maxLength: 'Phone number must not exceed limit of 20 characters.'
  },
  personalPhoneNumber:{
      required: 'Personal Phone number is required.',
      maxLength: 'Personal Phone number must not exceed limit of 20 characters.'
  },
  dateOfJoining:{
      required: 'Date of joining is required.',
      minLength: 'Date of joining  must be greater than today\'s date.'
  },
  dateOfBirth:{
      required: 'Date of birth is required.',
      maxLength: 'Date of birth  must be lesser than today\'s date.'
  },
  designationId:{
      required: 'Designation is required.',
  },
  roleId:{
      required: 'Role is required.',
  },
  username:{
      required: 'Username is required.',
      maxLength: 'Username must not exceed limit of 200 characters.'
  },
  password:{
      required: 'Password id required',
      minlength: 'Password length must be greater than or equal to 8.',
      maxLength: 'Password must not exceed limit of 200 characters.',
      pattern: 'Password must contain atleast one character(a/A to z/Z), one digit(1 to 9) and one special character(@)'
  },
  confirmPassword:{
      required: 'Confirm Password id Required',
      minlength: 'Confirm Password length must be greater than or equal to 8.',
      match: 'Password does not match',
      maxLength: 'Confirm Password must not exceed limit of 200 characters.',
      pattern: 'Confirm Password must contain atleast one character(a/A to z/Z), one digit(1 to 9) and one special character(@)'
  },
};

export const LOGIN_VALIDATION_MESSAGE = {
  employeeNo:{
      required: 'Employee No is required.',
  },
  password:{
      required: 'Password id required',
      minlength: 'Password length must be greater than or equal to 8.',
      maxLength: 'Password must not exceed limit of 200 characters.',
  }
}

export const FORGOTPASSWORD_VALIDATION_MESSAGE = {
  email:{
      required: 'Email is required.',
      email: 'Please enter a proper email.'
  },
}
