<template>
  <div id="cartC">
    <div
      v-for="index in widgetIndex"
      :key="index"
      v-bind:class="draggableclass"
    >
      <DxSortable
        :data="fileData[index - 1]"
        group="tasksGroup"
        :allow-drop-inside-item="true"
        @add="onAdd($event, index)"
        @remove="onRemove(index)"
      >
        <div style="height: 100%">
          <div v-if="fileData[index - 1]">
            <span class="numb" v-if="fileData[index - 1].name"
              >C{{ channelC_1_Class }}
            </span>
            <span class="numb_i" v-if="fileData[index - 1].name">
              <b-icon icon="folder" class="m-0"></b-icon>
              <b-icon icon="disc" class="m-0"></b-icon>
            </span>
            <div>
              <span class="numtext">{{ fileData[index - 1].name }}</span>
              <div class="numtext_b">
                {{ fileData[index - 1].categoryName }}
              </div>
              <div class="numtextdate">{{ fileData[index - 1].duration }}</div>
            </div>
            <div>
              <button
                id="closeBtn"
                type="button"
                class="btn btn-lg"
                @click="arrdelete(index)"
                style="background-color: transparent"
                v-if="fileData[index - 1].name"
              >
                <b-icon-x></b-icon-x>
              </button>
            </div>
          </div>
        </div>
      </DxSortable>
    </div>
  </div>
</template>

<script>
import { DxSortable } from "devextreme-vue/sortable";
export default {
  props: {
    widgetIndex: Number,
    channelC_1_Class: Number,
  },
  data() {
    return {
      selectData: [],
      fileData: [],
      oldIndex: null,
    };
  },
  mounted() {
    for (var i = 0; i < this.widgetIndex; i++) {
      this.fileData.push({});
    }
  },
  components: { DxSortable },
  computed: {
    draggableclass: function () {
      return {
        draggable: this.channelC_1_Class == 0,
        draggable_c1: this.channelC_1_Class == 1,
        draggable_c2: this.channelC_1_Class == 2,
        draggable_c3: this.channelC_1_Class == 3,
        draggable_c4: this.channelC_1_Class == 4,
      };
    },
  },
  methods: {
    arrdelete(index) {
      this.fileData.splice(index - 1, 1, {});
    },
    onAdd(e, index) {
      if (e.fromData == undefined) {
        this.fileData.splice(index - 1, 1, e.itemData);
      } else {
        this.fileData.splice(index - 1, 1, e.fromData);
      }
    },
    onRemove(index) {
      this.fileData.splice(index - 1, 1, {});
    },
  },
};
</script>

<style lang="scss" scoped>
#closeBtn {
  color: #008ecc;
  position: absolute;
  padding: 0;
  width: 50px;
  height: 40px;
  font-weight: 550;
  padding: 0px 0px 5px 150px;
  font-size: 20px;
}
.numtextdate {
  position: absolute;
  font-size: 11px;
  margin-left: 103px;
  margin-top: 38px;
}
.numtext {
  position: absolute;
  font-size: 14.5px;
  margin: 10px 10px 0px 13px;
  padding: 65px 0px 0px 5px;
}
.numtext_b {
  position: absolute;
  margin: 10px 10px 0px 13px;
  padding: 82px 0px 0px 5px;
  font-size: 13px;
}
.dx-icon-globe {
  position: absolute;
  font: 20px/1 DXIcons !important;
  margin: 130px 5px 100px 140px !important;
}
.numi {
  position: absolute;
  padding-left: 60px;
}
.numi > i {
  color: #d8d8d8;
}
.numb {
  background-color: rgb(85, 84, 84);
  color: white;
  position: absolute;
  margin-left: 18px;
  margin-top: 30px;
  padding: 2px;
  font-size: 15px;
}

.numb_i {
  position: absolute;

  margin-left: 50px;
  margin-top: 32px;
  font-size: 18px;
}
#cartC {
  height: 100%;
  display: grid;
  grid-template-columns: 1fr 1fr 1fr 1fr;
  grid-template-rows: 1fr 1fr 1fr 1fr;
}
.draggable {
  margin: 10px;
  background-color: #e9e9e9;
  border-radius: 3px;
}
.draggable_c1 {
  margin: 10px;
  background-color: #f8e5e5;
  border-radius: 1.5px;
}
.draggable_c2 {
  margin: 10px;
  background-color: #f3e8d9;
  border-radius: 1.5px;
}
.draggable_c3 {
  margin: 10px;
  background-color: #e1f8e1;
  border-radius: 1.5px;
}
.draggable_c4 {
  margin: 10px;
  background-color: #dfecf7;
  border-radius: 1.5px;
}
.dx-sortable {
  height: 100%;
}
</style>
