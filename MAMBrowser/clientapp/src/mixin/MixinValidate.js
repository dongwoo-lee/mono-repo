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
          },
          regDtm: {
            check_date: commonValidate.date,
          }
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
