export const VALIDATION_MESSAGE = {
  employeeName: {
    required: 'Employee name is required.',
    maxLength: 'Employee name must not exceed limit of 200 characters.'
  },
  email:{
      required: 'Email is required.',
      email: 'Please enter a proper email.',
      maxLength: 'Email must not exceed limit of 200 characters.'
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
  departmentId:{
    required: 'Department is required.',
  },
  employeeNo:{
    required: 'Employee No is required.',
  },
  roleId:{
      required: 'Role is required.',
  }
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

export const SKILL_MASTER_VALIDATION_MESSAGE = {
  title:{
    required: 'Title is required field'
  }
}
