<template>
  <transition name="modal" appear>
    <div id="delete-modal-mask">
      <div class="delete-modal-wrapper">
        <div class="delete-modal-container">
          <div class="modal-header">
            <slot name="header"> default header </slot>
          </div>

          <div class="delete-modal-body">
            <slot name="body"> default body </slot>
          </div>

          <div class="delete-modal-footer">
            <slot name="footer"> default footer </slot>
          </div>

          <b-overlay :show="show" no-wrap />
        </div>
      </div>
    </div>
  </transition>
</template>

<script>
export default {
  props: {
    show: {
      type: Boolean,
      default: false,
    },
  },
};
</script>

<style>
#delete-modal-mask {
  position: fixed;
  z-index: 9600;
  top: -4%;
  left: 0%;
  width: 100%;
  height: 110%;
  background-color: rgba(0, 0, 0, 0.5);
  display: table;
}

.delete-modal-wrapper {
  display: table-cell;
  vertical-align: middle;
}

.delete-modal-container {
  position: relative;
  margin: auto;
  width: 400px;
  margin-top: -400px;
  background-color: white;
  border-radius: 2px;
  /* box-shadow: 0 2px 8px rgba(0, 0, 0, 0.33); */
  transition: all 0.3s ease;
  font-family: "MBC 새로움 M";
}

.delete-modal-header h2 {
  color: #008ecc;
  font-family: "MBC 새로움 M";
}

.delete-modal-body {
  font-family: "MBC 새로움 M";
}

.delete-modal-default-button {
  float: right;
  position: absolute;
  top: 25px;
  right: 25px;
  border: 0;
  outline: 0;
  background-color: white;
  font-family: "MBC 새로움 M";
}
.delete-modal-footer {
  width: 400px;
  height: 83px;
  background-color: white;
  display: -webkit-box;
  display: -ms-flexbox;
  display: flex;
  -webkit-box-align: center;
  -ms-flex-align: center;
  align-items: center;
  -webkit-box-pack: end;
  -ms-flex-pack: end;
  justify-content: flex-end;
  padding: 1rem;
  border-top: 1px solid #dee2e6;
  border-bottom-right-radius: 0.3rem;
  border-bottom-left-radius: 0.3rem;
}
.modal-enter {
  opacity: 0;
}

.modal-leave-active {
  opacity: 0;
}

.modal-enter .modal-container,
.modal-leave-active .modal-container {
  -webkit-transform: scale(1.1);
  transform: scale(1.1);
}
</style>
