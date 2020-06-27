<template>
<div>
  <b-row>
    <b-colxx xxs="12">
      <piaf-breadcrumb :heading="$t('menu.waste-basket')"/>
      <div class="separator mb-5"></div>
    </b-colxx>
  </b-row>
  <b-row>
    <b-colxx xxs="12">
        <b-card class="mb-4" :title="$t('menu.waste-basket')" >
        </b-card>
    </b-colxx>
  </b-row>
  </div>
</template>

<script>
export default {
  data() {
    return {
      testData: '',
    }
  },
  mounted() {
    this.requestApi();
  },
  methods: {
    requestApi() {
      this.$http.get('/api/WeatherForecast/TestApi')
      .then((res) => {
        console.info(res);
      })
    }
  }
}
</script>