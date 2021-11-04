<template>
  <transition name="modal" appear>
    <div id="meta-modal-mask">
      <div class="meta-modal-wrapper">
        <div class="meta-modal-container">
          <div class="modal-header">
            <slot name="header">
              default header
            </slot>
            <button class="modal-default-button" @click="$emit('close')">
              <p class="h4 mb-2">
                <b-icon icon="x" class="icon" variant="danger"></b-icon>
              </p>
            </button>
          </div>

          <div class="meta-modal-body">
            <slot name="body">
              default body
            </slot>
          </div>

          <div class="meta-modal-footer">
            <slot name="footer"> </slot>
          </div>
        </div>
      </div>
    </div>
  </transition>
</template>

<script>
export default {};
</script>

<style>
#meta-modal-mask {
  position: fixed;
  z-index: 9600;
  top: -80px;
  left: -80px;
  width: 2000px;
  height: 120%;
  background-color: rgba(0, 0, 0, 0.5);
  display: table;
}

.meta-modal-wrapper {
  display: table-cell;
  vertical-align: middle;
}

.meta-modal-container {
  position: fixed;
  right: 450px;
  width: 1000px;
  height: 577px;
  margin-top: -320px;
  background-color: white;
  border-radius: 2px;
  /* box-shadow: 0 2px 8px rgba(0, 0, 0, 0.33); */
  transition: all 0.3s ease;
  font-family: Helvetica, Arial, sans-serif;
}

.modal-header h3 {
  margin-top: 10px;
  color: #008ecc;
}

.meta-modal-body {
  height: 470px;
  background-color: white;
}

.modal-default-button {
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
</style>
