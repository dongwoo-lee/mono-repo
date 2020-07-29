import commonValidate from '../utils/CommonValidate';

let mixinValidate = {
    validations: {
        searchItems: {
          brd_dt: {
            check_date: commonValidate.date,
          }, 
          start_dt: {
            check_date: commonValidate.date,
          },
          end_dt: {
            check_date: commonValidate.date,
          }
        },
    }
}

export default mixinValidate;