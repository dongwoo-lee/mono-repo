import Overlay from './Overlay.vue'

export default {
  install (Vue) {
    let Constr = Vue.extend(Overlay)
    let Overlay = new Constr()
    let vm = Overlay.$mount()
    document.querySelector('body').appendChild(vm.$el)
    const overlay = {
        enable: () => {
            Overlay.enable();
        },
        disable: () => {
            Overlay.disable();
        }
    };

    window.$overlay = overlay;
    Vue.$overlay = Vue.prototype.$Overlay = overlay;
  }
}
