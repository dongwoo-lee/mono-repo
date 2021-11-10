<template>
  <transition name="modal" appear>
    <div id="file-modal-mask">
      <div class="file-modal-wrapper">
        <div class="file-modal-container">
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

          <div class="file-modal-body">
            <slot name="body">
              default body
            </slot>
          </div>

          <!-- <div class="modal-footer">
            <slot name="footer"> </slot>
          </div> -->
        </div>
      </div>
    </div>
  </transition>
</template>

<script>
export default {};
</script>

<style>
#file-modal-mask {
  position: fixed;
  z-index: 9600;
  top: -80px;
  left: -80px;
  width: 2000px;
  height: 120%;
  background-color: rgba(0, 0, 0, 0.5);
  display: table;
}

.file-modal-wrapper {
  display: table-cell;
  vertical-align: middle;
}

.file-modal-container {
  position: fixed;
  right: 200px;
  width: 1400px;
  height: 800px;
  margin-top: -400px;
  background-color: white;
  border-radius: 2px;
  /* box-shadow: 0 2px 8px rgba(0, 0, 0, 0.33); */
  transition: all 0.3s ease;
  font-family: Helvetica, Arial, sans-serif;
}

.modal-header h2 {
  margin-top: 10px;
  color: #008ecc;
  font-weight: bolder;
}

.file-modal-body {
  height: 600px;
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
/* .modal-footer {
  margin-top: 10px;
  margin-bottom: -20px;
} */
</style>
