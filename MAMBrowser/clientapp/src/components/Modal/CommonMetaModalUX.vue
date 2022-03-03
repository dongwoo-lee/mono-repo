<template>
  <transition name="modal" appear>
    <div id="meta-modal-mask">
      <div class="meta-modal-wrapper">
        <div
          v-bind:class="[
            !isActive ? 'meta-modal-container' : 'meta-modal-container2',
          ]"
        >
          <div class="modal-header">
            <slot name="header"> default header </slot>
            <button class="modal-default-button" @click="$emit('close')">
              <p class="h4 mb-2">
                <b-icon icon="x" class="icon" variant="danger"></b-icon>
              </p>
            </button>
          </div>
          <div class="meta-modal-body">
            <slot name="body"> default body </slot>
          </div>

          <div
            v-bind:class="[
              !isActive ? 'meta-modal-footer' : 'meta-modal-footer2',
            ]"
          >
            <slot name="footer"> </slot>
          </div>
        </div>
      </div>
    </div>
  </transition>
</template>

<script>
export default {
  props: {
    isActive: {
      type: Boolean, // list data
      default: false,
    },
  },
};
</script>

<style>
#meta-modal-mask {
  position: fixed;
  z-index: 9600;
  top: -4%;
  left: 0%;
  width: 100%;
  height: 110%;
  background-color: rgba(0, 0, 0, 0.5);
  display: table;
}

.meta-modal-wrapper {
  display: table-cell;
  vertical-align: middle;
}

.meta-modal-container {
  position: relative;
  margin-top: 20px;
  width: 1000px;
  height: 660px;
  background-color: white;
  border-radius: 2px;
  /* box-shadow: 0 2px 8px rgba(0, 0, 0, 0.33); */
  transition: all 0.3s ease;
  font-family: "MBC 새로움 M";
}

.meta-modal-container2 {
  position: relative;
  margin-top: -240px;
  width: 1000px;
  height: 400px;
  background-color: white;
  border-radius: 2px;
  /* box-shadow: 0 2px 8px rgba(0, 0, 0, 0.33); */
  transition: all 0.3s ease;
  font-family: "MBC 새로움 M";
}

.modal-header h3 {
  margin-top: 10px;
  color: black;
  font-family: "MBC 새로움 M";
}

.meta-modal-body {
  font-family: "MBC 새로움 M";
  width: 1000px;
  height: 400px;
  background-color: white;
}

.modal-default-button {
  font-family: "MBC 새로움 M";
  float: right;
  position: absolute;
  top: 25px;
  right: 25px;
  border: 0;
  outline: 0;
  background-color: white;
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
.meta-modal-footer {
  width: 1000px;
  height: 83px;
  background-color: white;
}
.meta-modal-footer2 {
  font-family: "MBC 새로움 M";
  width: 1000px;
  height: 83px;
  background-color: white;
}
</style>
