import VueNotify from './Notify.vue'

export default {
  install (Vue, options = {}) {
    let Constr = Vue.extend(VueNotify)
    let Notify = new Constr()
    Notify.options = Object.assign(Notify.options, options)
    let vm = Notify.$mount()
    document.querySelector('body').appendChild(vm.$el)
    const notify = (type = 'success', title, message, options = {}) => {
      Notify.addItem(type, title, message, options)
    };
    window.$notify = notify;
    Vue.$notify = Vue.prototype.$notify = notify;
  }
}
