import commonValidate from '../utils/CommonValidate';

let mixinValidate = {
    validations: {
        searchItems: {
          brd_dt: {
            check_date: commonValidate.date,
            required: commonValidate.required,
          }, 
          start_dt: {
            check_date: commonValidate.date,
            required: commonValidate.required,
          },
          end_dt: {
            check_date: commonValidate.date,
            required: commonValidate.required,
          },
          regDtm: {
            check_date: commonValidate.date,
            required: commonValidate.required,
          },
        },
        metaData: {
          title: {
            required: commonValidate.required,
          },
          memo: {
            required: commonValidate.required,
          },
          mediaCD: {
            required: commonValidate.required,
          },
          categoryCD: {
            required: commonValidate.required,
          }
        },
        title: {
          required: commonValidate.required,
        },
        memo: {
          required: commonValidate.required,
        },
        type: {
          required: commonValidate.required,
        },
        mediaCD: {
          required: commonValidate.required,
        },
        categoryCD: {
          required: commonValidate.required,
        }
    }
}

export default mixinValidate;
