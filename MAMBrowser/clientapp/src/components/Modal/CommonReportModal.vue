<template>
  <transition name="modal" appear>
    <div id="report-modal-mask">
      <div class="report-modal-wrapper">
        <div class="report-modal-container">
          <div class="modal-header">
            <slot name="header"> default header </slot>
            <button class="report-modal-default-button" @click="$emit('close')">
              <p class="h4 mb-2">
                <b-icon icon="x" class="icon" variant="danger"></b-icon>
              </p>
            </button>
          </div>

          <div class="report-modal-body">
            <slot name="body"> default body </slot>
          </div>

          <div class="report-modal-footer">
            <slot name="footer"> default footer </slot>
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
#report-modal-mask {
  position: fixed;
  z-index: 9600;
  top: 0%;
  left: 0%;
  width: 100%;
  height: 100%;
  background-color: rgba(0, 0, 0, 0.5);
  display: table;
}

.report-modal-wrapper {
  display: table-cell;
  vertical-align: middle;
}

.report-modal-container {
  position: relative;
  top: -170px;
  margin: auto;
  width: 400px;
  height: 300px;
  background-color: white;
  border-radius: 2px;
  /* box-shadow: 0 2px 8px rgba(0, 0, 0, 0.33); */
  transition: all 0.3s ease;
  font-family: "MBC 새로움 M";
}

.report-modal-header h2 {
  margin-top: 10px;
  color: #008ecc;
  font-family: "MBC 새로움 M";
}

.report-modal-body {
  height: 400px;
  background-color: white;
  font-family: "MBC 새로움 M";
}

.report-modal-default-button {
  float: right;
  position: absolute;
  top: 25px;
  right: 25px;
  border: 0;
  outline: 0;
  background-color: white;
  font-family: "MBC 새로움 M";
}
.report-modal-footer {
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
