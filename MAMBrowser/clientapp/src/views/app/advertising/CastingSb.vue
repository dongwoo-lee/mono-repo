<template>
<div>
  <b-row>
    <b-colxx xxs="12">
      <piaf-breadcrumb :heading="$t('menu.casting-sb')"/>
      <div class="separator mb-5"></div>
    </b-colxx>
  </b-row>
  <b-row>
    <b-colxx xxs="12">
        <b-card class="mb-4" :title="$t('menu.casting-sb')">
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
}
</script>